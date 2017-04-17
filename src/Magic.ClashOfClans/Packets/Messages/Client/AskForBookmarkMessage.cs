using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AskForBookmarkMessage : Message
    {
        public AskForBookmarkMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            new BookmarkListMessage(Client).Send();
        }
    }
}