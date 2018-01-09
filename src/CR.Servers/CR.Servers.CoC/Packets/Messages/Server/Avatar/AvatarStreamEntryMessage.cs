namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    using CR.Servers.CoC.Logic;

    internal class AvatarStreamEntryMessage : Message
    {
        internal MailEntry StreamEntry;

        public AvatarStreamEntryMessage(Device Device) : base(Device)
        {
        }

        internal override short Type => 24412;

        internal override void Encode()
        {
            this.StreamEntry.Encode(this.Data);
        }
    }
}