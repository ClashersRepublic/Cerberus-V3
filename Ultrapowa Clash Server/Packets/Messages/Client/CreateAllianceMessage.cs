using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Commands.Client;
using UCS.PacketProcessing.Messages.Server;
using UCS.PacketProcessing.Commands.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class CreateAllianceMessage : Message
    {
        private int m_vAllianceBadgeData;
        private string m_vAllianceDescription;
        private string m_vAllianceName;
        private int m_vAllianceOrigin;
        private int m_vAllianceType;
        private int m_vRequiredScore;
        private int m_vWarFrequency;
        private byte m_vWarAndFriendlyStatus;

        public CreateAllianceMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {

        }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(GetData())))
            {
                m_vAllianceName = br.ReadString();
                m_vAllianceDescription = br.ReadString();
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
            if (m_vAllianceName == null)
                m_vAllianceName = "Clan";

            if (m_vAllianceName.Length < 16 || m_vAllianceName.Length < 1)
            {
                if (m_vAllianceDescription.Length < 259 || m_vAllianceDescription.Length < 0)
                {
                    if (m_vAllianceBadgeData < 1 || (long)m_vAllianceBadgeData < 10000000000L)
                    {
                        if (m_vAllianceType < 0 || m_vAllianceType < 10)
                        {
                            if (m_vRequiredScore < 0 || m_vRequiredScore < 4201)
                            {
                                if (m_vWarFrequency < 0 || m_vWarFrequency < 10)
                                {
                                    if (m_vAllianceOrigin < 0 || m_vAllianceOrigin < 42000000)
                                    {
                                        if ((int)m_vWarAndFriendlyStatus < 0 || (int)m_vWarAndFriendlyStatus < 5)
                                        {

                                            Alliance alliance = ObjectManager.CreateAlliance();
                                            alliance.SetAllianceName(m_vAllianceName);
                                            alliance.SetAllianceDescription(m_vAllianceDescription);
                                            alliance.SetAllianceType(m_vAllianceType);
                                            alliance.SetRequiredScore(m_vRequiredScore);
                                            alliance.SetAllianceBadgeData(m_vAllianceBadgeData);
                                            alliance.SetAllianceOrigin(m_vAllianceOrigin);
                                            alliance.SetWarFrequency(m_vWarFrequency);
                                            alliance.SetWarAndFriendlytStatus(m_vWarAndFriendlyStatus);
                                            level.GetPlayerAvatar().SetAllianceId(alliance.GetAllianceId());
                                            AllianceMemberEntry entry = new AllianceMemberEntry(level.GetPlayerAvatar().GetId());
                                            entry.SetRole(2);
                                            alliance.AddAllianceMember(entry);
                                            JoinedAllianceCommand Command1 = new JoinedAllianceCommand();
                                            Command1.SetAlliance(alliance);
                                            AllianceRoleUpdateCommand Command2 = new AllianceRoleUpdateCommand();
                                            Command2.SetAlliance(alliance);
                                            Command2.SetRole(2);
                                            Command2.Tick(level);
                                            var a = new AvailableServerCommandMessage(Client);
                                            a.SetCommandId(1);
                                            a.SetCommand(Command1);
                                            var c = new AvailableServerCommandMessage(Client);
                                            c.SetCommandId(8);
                                            c.SetCommand(Command2);
                                            a.Send();
                                            c.Send();
                                        }
                                        else
                                            ResourcesManager.DisconnectClient(Client);
                                    }
                                    else
                                        ResourcesManager.DisconnectClient(Client);
                                }
                                else
                                    ResourcesManager.DisconnectClient(Client);
                            }
                            else
                                ResourcesManager.DisconnectClient(Client);
                        }
                        else
                            ResourcesManager.DisconnectClient(Client);
                    }
                    else
                        ResourcesManager.DisconnectClient(Client);
                }
                else
                    ResourcesManager.DisconnectClient(Client);
            }
            else
                ResourcesManager.DisconnectClient(Client);
        }
    }
}
