using System;
using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.DataSlots;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class RemoveFromBookmarkMessage : Message
    {
        private long id;

        public RemoveFromBookmarkMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
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
            BookmarkSlot al = level.GetPlayerAvatar().BookmarkedClan.Find((Predicate<BookmarkSlot>) (a => a.Value == id));
            if (al != null)
            level.GetPlayerAvatar().BookmarkedClan.Remove(al);
            new BookmarkRemoveAllianceMessage(Client).Send();
        } 
    }
}