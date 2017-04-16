using UCS.Helpers;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class NewsSeenMessage : Message
    {
        public NewsSeenMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }
    }
}