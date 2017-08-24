using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Avatar_Profile_Data : Message
    {
        internal Level Player;
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
