using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class News_Seen : Message
    {
        internal int Unknown;

        public News_Seen(Device device) : base(device)
        {
        }

        public override void Decode()
        {
            Unknown = Reader.ReadInt32();
        }

        public override void Process()
        {
            new News_Seen_Response(Device).Send();
        }
    }
}