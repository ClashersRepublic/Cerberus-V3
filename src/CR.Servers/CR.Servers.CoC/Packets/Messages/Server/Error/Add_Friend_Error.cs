using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Error
{
    internal class Add_Friend_Error : Message
    {
        internal override short Type => 20112;

        public Add_Friend_Error(Device Device) : base(Device)
        {
            
        }

        internal AddFriendErrorReason Reason;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
        }
    }
}
