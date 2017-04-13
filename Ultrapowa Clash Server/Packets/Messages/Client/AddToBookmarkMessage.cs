using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.DataSlots;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class AddToBookmarkMessage : Message
    {
        private long id;

        public AddToBookmarkMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(GetData())))
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