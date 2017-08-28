using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Bookmark_Add_Response : Message
    {
        internal bool Response;

        public Bookmark_Add_Response(Device device) : base(device)
        {
            Identifier = 24343;
        }

        public override void Encode()
        {
            Data.AddBool(Response);
        }
    }
}