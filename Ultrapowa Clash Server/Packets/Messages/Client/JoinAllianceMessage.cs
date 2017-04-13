using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Commands.Server;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class JoinAllianceMessage : Message
    {
        private long m_vAllianceId;

        public JoinAllianceMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                m_vAllianceId = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            JoinAllianceMessage joinAllianceMessage = this;
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(m_vAllianceId);
                if (alliance == null || alliance.IsAllianceFull())
                  return;
                    level.GetPlayerAvatar().SetAllianceId(alliance.GetAllianceId());
                    AllianceMemberEntry entry = new AllianceMemberEntry(level.GetPlayerAvatar().GetId());
                    entry.SetRole(1);
                    alliance.AddAllianceMember(entry);

                    JoinedAllianceCommand Command1 = new JoinedAllianceCommand();
                    Command1.SetAlliance(alliance);

                    AllianceRoleUpdateCommand Command2 = new AllianceRoleUpdateCommand();
                    Command2.SetAlliance(alliance);
                    Command2.SetRole(1);
                    Command2.Tick(level);

                    var a = new AvailableServerCommandMessage(Client);
                    a.SetCommandId(1);
                    a.SetCommand(Command1);

                    var d = new AvailableServerCommandMessage(Client);
                    d.SetCommandId(8);
                    d.SetCommand(Command2);

                    a.Send();
                    d.Send();
                    
                     new AllianceStreamMessage(Client, alliance).Send();
            }
              catch (Exception ex)
              {
              }
}
    }
}