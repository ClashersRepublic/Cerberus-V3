using System;
using System.IO;
using System.Text;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.StreamEntry;
using Magic.PacketProcessing.Messages.Server;
using  Magic.PacketProcessing.Commands.Server;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class EditClanSettingsMessage : Message
    {
        private int m_vAllianceBadgeData;
        private string m_vAllianceDescription;
        private int m_vAllianceOrigin;
        private int m_vAllianceType;
        private int m_vRequiredScore;
        private int m_vWarFrequency;
        private byte m_vWarAndFriendlyStatus;

        public EditClanSettingsMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader (new MemoryStream(Data)))
            {
                m_vAllianceDescription = br.ReadString();
                br.ReadInt32();
                m_vAllianceBadgeData = br.ReadInt32();
                m_vAllianceType = br.ReadInt32();
                m_vRequiredScore = br.ReadInt32();
                m_vWarFrequency = br.ReadInt32();
                m_vAllianceOrigin = br.ReadInt32();
                m_vWarAndFriendlyStatus = br.ReadByte();
            }
        }

        public override void Process(Level level)
        {
            EditClanSettingsMessage clanSettingsMessage = this;
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
                if (alliance != null)
                {
                    if (clanSettingsMessage.m_vAllianceDescription.Length < 259 || clanSettingsMessage.m_vAllianceDescription.Length < 0)
                    {
                        if (clanSettingsMessage.m_vAllianceBadgeData < 1 || (long)clanSettingsMessage.m_vAllianceBadgeData < 10000000000L)
                        {
                            if (clanSettingsMessage.m_vAllianceType < 0 || clanSettingsMessage.m_vAllianceType < 10)
                            {
                                if (clanSettingsMessage.m_vRequiredScore < 0 || clanSettingsMessage.m_vRequiredScore < 4201)
                                {
                                    if (clanSettingsMessage.m_vWarFrequency < 0 || clanSettingsMessage.m_vWarFrequency < 10)
                                    {
                                        if (clanSettingsMessage.m_vAllianceOrigin < 0 || clanSettingsMessage.m_vAllianceOrigin < 42000000)
                                        {
                                            if ((int)clanSettingsMessage.m_vWarAndFriendlyStatus < 0 || (int)clanSettingsMessage.m_vWarAndFriendlyStatus < 5)
                                            {
                                                alliance.SetAllianceDescription(m_vAllianceDescription);
                                                alliance.SetAllianceBadgeData(m_vAllianceBadgeData);
                                                alliance.SetAllianceType(m_vAllianceType);
                                                alliance.SetRequiredScore(m_vRequiredScore);
                                                alliance.SetWarFrequency(m_vWarFrequency);
                                                alliance.SetAllianceOrigin(m_vAllianceOrigin);
                                                alliance.SetWarAndFriendlytStatus(m_vWarAndFriendlyStatus);
                                                ClientAvatar avatar = level.GetPlayerAvatar();
                                                avatar.GetAllianceId();
                                                AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                                                eventStreamEntry.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                                eventStreamEntry.SetSender(avatar);
                                                eventStreamEntry.SetEventType(10);
                                                eventStreamEntry.SetAvatarId(avatar.GetId());
                                                eventStreamEntry.SetAvatarName(avatar.GetAvatarName());
                                                eventStreamEntry.SetSenderId(avatar.GetId());
                                                eventStreamEntry.SetSenderName(avatar.GetAvatarName());
                                                alliance.AddChatMessage(eventStreamEntry);
                                                AllianceSettingChangedCommand Command = new AllianceSettingChangedCommand();
                                                Command.SetAlliance(alliance);
                                                Command.SetPlayer(level);
                                                var availableServerCommandMessage = new AvailableServerCommandMessage(level.GetClient());
                                                availableServerCommandMessage.SetCommandId(6);
                                                availableServerCommandMessage.SetCommand(Command);
                                                availableServerCommandMessage.Send();
                                                foreach (AllianceMemberEntry allianceMember in alliance.GetAllianceMembers())
                                                {
                                                    Level player = ResourcesManager.GetPlayer(allianceMember.GetAvatarId(), false);
                                                    if (ResourcesManager.IsPlayerOnline(player))
                                                    {
                                                        var p = new AllianceStreamEntryMessage(player.GetClient());
                                                        AllianceEventStreamEntry eventStreamEntry1 = eventStreamEntry;
                                                        p.SetStreamEntry(eventStreamEntry1);
                                                        p.Send();
                                                    }
                                                }
                                                List<AllianceMemberEntry>.Enumerator enumerator = new List<AllianceMemberEntry>.Enumerator();
                                                DatabaseManager.Instance.Save(alliance);
                                                eventStreamEntry = (AllianceEventStreamEntry) null;
                                            }
                                            else
                                                ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                                        }
                                        else
                                            ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                                    }
                                    else
                                        ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                                }
                                else
                                    ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                            }
                            else
                                ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                        }
                        else
                            ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                    }
                    else
                        ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                }
                alliance = (Alliance) null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
