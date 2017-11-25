using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Data : Message
    {
        internal override short Type => 24301;

        public Alliance_Data(Device Device) : base(Device)
        {
        }

        internal Alliance Alliance;

        internal override void Encode()
        {
            this.Alliance.Encode(this.Data);
        }
    }
}
