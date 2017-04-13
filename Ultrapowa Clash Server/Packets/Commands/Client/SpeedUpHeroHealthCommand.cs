using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class SpeedUpHeroHealthCommand : Command
    {
        public SpeedUpHeroHealthCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
        }
    }
}