using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.DataSlots;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AddToBookmarkMessage : Message
    {
        private long id;

        public AddToBookmarkMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(Data)))
            {
                id = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            level.GetPlayerAvatar().BookmarkedClan.Add(new BookmarkSlot(id));
            new BookmarkAddAllianceMessage(level.GetClient()).Send();
        }
    }
}