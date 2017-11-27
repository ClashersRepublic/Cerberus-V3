using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Stream : Message
    {
        internal override short Type => 24411;

        public Avatar_Stream(Device Device) : base (Device)
        {
        }


        internal override void Encode()
        {
            this.Device.GameMode.Level.Player.Inbox.Encode(this.Data);
        }
    }
}
