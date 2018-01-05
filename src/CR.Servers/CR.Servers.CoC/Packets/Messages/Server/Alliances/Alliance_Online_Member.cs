namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Alliance_Online_Member : Message
    {
        internal int Connected;
        internal int TotalMember;

        public Alliance_Online_Member(Device Device) : base(Device)
        {
        }

        internal override short Type => 20207;

        internal override void Encode()
        {
            this.Data.AddVInt(this.Connected);
            this.Data.AddVInt(this.TotalMember);
        }
    }
}