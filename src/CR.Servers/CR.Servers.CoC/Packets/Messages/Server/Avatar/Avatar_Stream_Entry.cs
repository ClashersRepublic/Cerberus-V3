namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    using CR.Servers.CoC.Logic;

    internal class Avatar_Stream_Entry : Message
    {
        internal MailEntry StreamEntry;

        public Avatar_Stream_Entry(Device Device) : base(Device)
        {
        }

        internal override short Type => 24412;

        internal override void Encode()
        {
            this.StreamEntry.Encode(this.Data);
        }
    }
}