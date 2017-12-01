using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Create_Fail : Message
    {
        internal override short Type => 24332;

        public Alliance_Create_Fail(Device Device) : base(Device)
        {

        }

        internal AllianceErrorReason Error;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Error);
        }
    }
}
