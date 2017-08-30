using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class News_Seen_Response : Message
    {
        internal News_Seen_Response(Device Device) : base(Device)
        {
            Identifier = 24445;
        }

        public override void Encode()
        {
            Data.AddInt(0);
        }
    }
}
