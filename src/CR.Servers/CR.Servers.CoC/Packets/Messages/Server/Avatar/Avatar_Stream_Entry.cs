using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Stream_Entry : Message
    {
        internal override short Type => 24412;

        public Avatar_Stream_Entry(Device Device) : base (Device)
        {
        }

        internal MailEntry StreamEntry;

        internal override void Encode()
        {
            this.StreamEntry.Encode(this.Data);
        }
    }
}
