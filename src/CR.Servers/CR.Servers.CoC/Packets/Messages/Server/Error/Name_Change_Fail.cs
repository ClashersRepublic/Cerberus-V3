namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Enums;
    using CR.Servers.Extensions.List;

    internal class Name_Change_Fail : Message
    {
        internal NameErrorReason Error;

        public Name_Change_Fail(Device Device) : base(Device)
        {
        }

        internal override short Type => 20205;

        internal override void Encode()
        {
            this.Data.AddInt((int) this.Error);
        }
    }
}