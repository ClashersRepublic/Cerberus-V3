using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;
using UCS.Packets.Messages.Server;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class TogglePlayerWarStateCommand : Command
    {
        public TogglePlayerWarStateCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            //TODO
        }
    }
}