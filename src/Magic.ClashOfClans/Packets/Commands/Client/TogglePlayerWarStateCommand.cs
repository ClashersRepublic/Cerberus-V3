using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.PacketProcessing.Commands.Client
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