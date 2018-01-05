namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Clan;

    internal class Alliance_Stream_Entry : Message
    {
        internal StreamEntry StreamEntry;

        public Alliance_Stream_Entry(Device Device) : base(Device)
        {
        }

        internal override short Type => 24312;

        internal override void Encode()
        {
            this.StreamEntry.RequesterId = this.Device.GameMode.Level.Player.UserId;
            this.StreamEntry.Encode(this.Data);
        }
    }
}