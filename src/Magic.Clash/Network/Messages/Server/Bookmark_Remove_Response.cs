using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Bookmark_Remove_Response : Message
    {
        internal bool Response;

        public Bookmark_Remove_Response(Device device) : base(device)
        {
            Identifier = 24344;
        }

        public override void Encode()
        {
            Data.AddBool(Response);
        }
    }
}