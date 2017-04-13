using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.AvatarStreamEntry;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class SendAllianceMailCommand : Command
    {
        internal string m_vMailContent;

        public SendAllianceMailCommand(PacketReader br)
        {
            m_vMailContent = br.ReadString();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            SendAllianceMailCommand allianceMailCommand = this;
            try
            {
                ClientAvatar avatar = level.GetPlayerAvatar();
                long allianceId = avatar.GetAllianceId();
                if (allianceId > 0L)
                {
                    Alliance alliance = ObjectManager.GetAlliance(allianceId);
                    if (alliance != null)
                    {
                        AllianceMailStreamEntry allianceMailStreamEntry1 = new AllianceMailStreamEntry();
                        allianceMailStreamEntry1.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        allianceMailStreamEntry1.SetAvatar(avatar);
                        allianceMailStreamEntry1.SetIsNew((byte)2);
                        allianceMailStreamEntry1.SetSenderId(avatar.GetId());
                        allianceMailStreamEntry1.SetAllianceId(allianceId);
                        allianceMailStreamEntry1.SetAllianceBadgeData(alliance.GetAllianceBadgeData());
                        allianceMailStreamEntry1.SetAllianceName(alliance.GetAllianceName());
                        allianceMailStreamEntry1.SetMessage(m_vMailContent);
                        foreach (Level onlinePlayer in ResourcesManager.GetOnlinePlayers())
                        {
                            if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == allianceId)
                            {
                                AvatarStreamEntryMessage Message = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                                AllianceMailStreamEntry allianceMailStreamEntry2 = allianceMailStreamEntry1;
                                Message.SetAvatarStreamEntry((UCS.Logic.AvatarStreamEntry.AvatarStreamEntry)allianceMailStreamEntry2);
                                Message.Send();
                            }
                        }
                    }
                }
                avatar = (ClientAvatar) null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}