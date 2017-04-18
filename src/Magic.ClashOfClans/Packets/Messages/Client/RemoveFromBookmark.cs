using System;
using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.DataSlots;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class RemoveFromBookmarkMessage : Message
    {
        private long id;

        public RemoveFromBookmarkMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
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
            BookmarkSlot al = level.Avatar.BookmarkedClan.Find((Predicate<BookmarkSlot>)(a => a.Value == id));
            if (al != null)
                level.Avatar.BookmarkedClan.Remove(al);
            new BookmarkRemoveAllianceMessage(Client).Send();
        }
    }
}