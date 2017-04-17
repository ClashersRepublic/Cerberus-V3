using System.IO;
using Magic.Helpers;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class NewShopItemsSeenCommand : Command
    {
        public uint NewShopItemNumber;
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        public NewShopItemsSeenCommand(PacketReader br)
        {
           br.ReadInt32();
           br.ReadInt32();
           br.ReadInt32();
           br.ReadInt32();
        }
    }
}