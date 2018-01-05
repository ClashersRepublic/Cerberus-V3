namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Alliance_Member_State : Message
    {
        public Alliance_Member_State(Device Device) : base(Device)
        {
            this.Version = 9;
        }

        internal override short Type => 20208;

        internal override void Encode()
        {
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
        }
    }
}