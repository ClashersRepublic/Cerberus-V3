using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class RenameQuickTrainCommand : Command
    {
        public int SlotID;
        public string SlotName;
        public int Tick;

        public RenameQuickTrainCommand(PacketReader br)
        {
            SlotID = br.ReadInt32();
            SlotName = br.ReadString();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}