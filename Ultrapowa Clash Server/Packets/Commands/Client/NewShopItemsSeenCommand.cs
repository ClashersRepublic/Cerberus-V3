using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
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