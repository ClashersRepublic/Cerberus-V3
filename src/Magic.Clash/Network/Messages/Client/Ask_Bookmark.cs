using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Ask_Bookmark : Message
    {
        public Ask_Bookmark(Device device, Reader reader) : base(device, reader)
        {
            // Ask_Bookmark.
        }

        public override void Process()
        {
            new Bookmark_Full_Entry(Device).Send();
        }
    }
}
