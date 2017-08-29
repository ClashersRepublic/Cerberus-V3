using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Invitation_SendFailed : Message
    {
        internal Alliance_Invite Response;

        public Alliance_Invitation_SendFailed(Device device) : base(device)
        {
            Identifier = 24321;
        }

        public override void Encode()
        {
            Data.AddInt((int) Response);
        }
    }
}
