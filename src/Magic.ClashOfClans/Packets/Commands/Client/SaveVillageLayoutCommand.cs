using System.IO;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class SaveVillageLayoutCommand : Command
    {
        public SaveVillageLayoutCommand(PacketReader br)
        {
            br.Read();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}
