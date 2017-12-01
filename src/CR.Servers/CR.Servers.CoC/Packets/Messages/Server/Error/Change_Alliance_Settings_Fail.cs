using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Change_Alliance_Settings_Fail : Message
    {
        internal override short Type => 24333;

        public Change_Alliance_Settings_Fail(Device Device) : base(Device)
        {

        }


        internal override void Encode()
        {
            this.Data.AddInt((int) AllianceErrorReason.InvalidDescription);
        }
    }
}
