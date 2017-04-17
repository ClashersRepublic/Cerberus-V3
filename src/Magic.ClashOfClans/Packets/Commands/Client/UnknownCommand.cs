using System.IO;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class UnknownCommand : Command
    {
        public static int Tick;
        public static int Unknown1;

        public UnknownCommand(PacketReader br)
        {
        }

        public override void Execute(Level level)
        {
        }
    }
}