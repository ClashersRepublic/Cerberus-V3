using System.IO;
using Magic.Helpers;

namespace Magic.PacketProcessing.Commands.Client
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