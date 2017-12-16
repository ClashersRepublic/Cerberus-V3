using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Member_State : Message
    {
        internal override short Type => 20208;

        public Alliance_Member_State(Device Device) : base(Device)
        {
            this.Version = 9;
        }

        internal override void Encode()
        {
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
        }
    }
}
