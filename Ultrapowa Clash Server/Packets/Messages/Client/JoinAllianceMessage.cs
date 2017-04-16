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
            // Space
        }

        public override void Decode()
        {
            m_vAllianceId = Reader.ReadInt64();
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance == null || alliance.IsAllianceFull())
                return;

            level.GetPlayerAvatar().SetAllianceId(alliance.GetAllianceId());
            var entry = new AllianceMemberEntry(level.GetPlayerAvatar().GetId());
            entry.SetRole(1);
            alliance.AddAllianceMember(entry);

            var jaCommand = new JoinedAllianceCommand();
            jaCommand.SetAlliance(alliance);

            var aruCommand = new AllianceRoleUpdateCommand();
            aruCommand.SetAlliance(alliance);
            aruCommand.SetRole(1);
            aruCommand.Tick(level);

            var asCommand1 = new AvailableServerCommandMessage(Client);
            asCommand1.SetCommandId(1);
            asCommand1.SetCommand(jaCommand);

            var asCommand2 = new AvailableServerCommandMessage(Client);
            asCommand2.SetCommandId(8);
            asCommand2.SetCommand(aruCommand);

            asCommand1.Send();
            asCommand2.Send();

            new AllianceStreamMessage(Client, alliance).Send();
        }
    }
}