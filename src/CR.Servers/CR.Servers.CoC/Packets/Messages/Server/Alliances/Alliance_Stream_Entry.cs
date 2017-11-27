using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Stream_Entry : Message
    {
        internal override short Type => 24312;

        public Alliance_Stream_Entry(Device Device) : base(Device)
        {
        }

        internal Alliance StreamEntry;

        internal override void Encode()
        {
            this.StreamEntry.Encode(this.Data);
        }
    }
}
