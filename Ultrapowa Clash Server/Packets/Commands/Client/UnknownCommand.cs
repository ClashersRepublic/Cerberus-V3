using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
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