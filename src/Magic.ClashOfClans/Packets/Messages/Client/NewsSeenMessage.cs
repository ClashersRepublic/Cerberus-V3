using Magic.Helpers;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class NewsSeenMessage : Message
    {
        public NewsSeenMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }
    }
}