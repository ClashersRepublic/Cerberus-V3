using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Name_Change_Fail : Message
    {
        internal override short Type => 20205;

        public Name_Change_Fail(Device Device) : base(Device)
        {
            
        }

        internal NameErrorReason Error;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Error);
        }
    }
}
