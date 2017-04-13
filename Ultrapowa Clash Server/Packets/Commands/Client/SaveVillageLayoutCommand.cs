using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
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
