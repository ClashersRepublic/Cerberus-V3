using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.DataSlots;

namespace Magic.PacketProcessing.Messages.Server
{
    // Packet 24340
    internal class BookmarkMessage : Message
    {
        public ClientAvatar player { get; set; }
        public int i;

        public BookmarkMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24340;
            player = client.Level.Avatar;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var list = new List<byte>();
            List<BookmarkSlot> rem = new List<BookmarkSlot>();
            Parallel.ForEach((player.BookmarkedClan), (p, l) =>
            {
                Alliance a = ObjectManager.GetAlliance(p.Value);
                if (a != null)
                {
                    list.AddInt64(p.Value);
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

            // xD
            Parallel.ForEach((rem), (im, l) =>
             {
                 player.BookmarkedClan.RemoveAll(t => t == im);
                 l.Stop();
             });
        }
    }
}
