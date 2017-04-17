using System.IO;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class ClientServerTickCommand : Command
    {
        public int Tick;
        public int Unknown1;

        public ClientServerTickCommand(PacketReader br)
        {
            Unknown1 = br.ReadInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}