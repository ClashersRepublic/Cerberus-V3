using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.DataSlots;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 24341
    internal class BookmarkListMessage : Message
    {
        public ClientAvatar player { get; set; }
        public int i;

        public BookmarkListMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24341;
            player = client.Level.Avatar;
            i = 0;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var list = new List<byte>();
            var rem = new List<BookmarkSlot>();
            Parallel.ForEach((player.BookmarkedClan), (p, l) =>
            {
                Alliance a = ObjectManager.GetAlliance(p.Value);
                if (a != null)
                {
                    list.AddRange(ObjectManager.GetAlliance(p.Value).EncodeFullEntry());
                    i++;
                }
                else
                {
                    rem.Add(p);
                    if (i > 0)
                        i--;
                }
                l.Stop();
            });
            data.AddInt32(i);
            data.AddRange(list);
            Encrypt(data.ToArray());
            Parallel.ForEach((rem), (im, l) =>
            {
                player.BookmarkedClan.RemoveAll(t => t == im);
                l.Stop();
            });
        }
    }
}
