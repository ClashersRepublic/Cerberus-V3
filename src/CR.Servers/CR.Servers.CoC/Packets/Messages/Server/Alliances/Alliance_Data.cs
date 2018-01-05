namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Clan;

    internal class Alliance_Data : Message
    {
        internal Alliance Alliance;

        public Alliance_Data(Device Device) : base(Device)
        {
        }

        internal override short Type => 24301;

        internal override void Encode()
        {
            this.Alliance.Encode(this.Data);
        }
    }
}