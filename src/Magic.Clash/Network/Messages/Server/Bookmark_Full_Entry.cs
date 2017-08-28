using System.Collections.Generic;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Bookmark_Full_Entry : Message
    {
        internal List<long> ToRemove;

        public Bookmark_Full_Entry(Device device) : base(device)
        {
            Identifier = 24341;
        }

        public override void Encode()
        {
            var i = 0;
            var Avatar = Device.Player.Avatar;

            ToRemove = new List<long>();
            var list = new List<byte>();

            foreach (var id in Avatar.Bookmarks)
            {
                var a = ObjectManager.GetAlliance(id);
                if (a != null)
                {
                    list.AddRange(a.ToBytes);
                    i++;
                }
                else
                {
                    ToRemove.Add(id);
                }
            }
            Data.AddInt(i);
            Data.AddRange(list.ToArray());
        }

        public override void Process()
        {
            foreach (var id in ToRemove)
                Device.Player.Avatar.Bookmarks.RemoveAll(t => t == id);
        }
    }
}
   