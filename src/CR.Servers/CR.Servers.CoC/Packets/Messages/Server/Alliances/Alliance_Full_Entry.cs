namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Clan;
    using CR.Servers.Extensions.List;

    internal class Alliance_Full_Entry : Message
    {
        internal Alliance Alliance;

        public Alliance_Full_Entry(Device Device) : base(Device)
        {
        }

        internal override short Type => 24324;

        internal override void Encode()
        {
            this.Data.AddString(this.Alliance.Description);
            this.Data.AddInt((int) this.Alliance.WarState);
            this.Data.AddInt(0);

            this.Data.AddByte(0);
            //this.Data.AddLong(WarID);

            this.Data.Add(0);
            this.Data.AddInt(0);
            this.Alliance.Header.Encode(this.Data);
        }
    }
}