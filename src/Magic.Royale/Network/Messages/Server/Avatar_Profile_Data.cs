using Magic.Royale.Core;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic;

namespace Magic.Royale.Network.Messages.Server
{
    internal class Avatar_Profile_Data : Message
    {
        internal Avatar Player;
        internal long UserID;

        public Avatar_Profile_Data(Device device) : base(device)
        {
            Identifier = 24334;
        }

        public override void Encode()
        {
        }
    }
}
