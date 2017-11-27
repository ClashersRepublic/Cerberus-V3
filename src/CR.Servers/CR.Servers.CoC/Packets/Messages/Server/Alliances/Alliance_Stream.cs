using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Stream : Message
    {
        internal override short Type => 24311;

        public Alliance_Stream(Device Device) : base(Device)
        {
        }


        internal Alliance Alliance;

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Alliance.Streams.Encode(this.Data);
        }
    }
}