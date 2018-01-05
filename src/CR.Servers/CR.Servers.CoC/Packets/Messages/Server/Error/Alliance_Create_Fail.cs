namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Enums;
    using CR.Servers.Extensions.List;

    internal class Alliance_Create_Fail : Message
    {
        internal AllianceErrorReason Error;

        public Alliance_Create_Fail(Device Device) : base(Device)
        {
        }

        internal override short Type => 24332;

        internal override void Encode()
        {
            this.Data.AddInt((int) this.Error);
        }
    }
}