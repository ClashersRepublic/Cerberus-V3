using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Online_Member : Message
    {
        internal override short Type => 20207;

        public Alliance_Online_Member(Device Device) : base(Device)
        {
        }

        internal int Connected;
        internal int TotalMember;

        internal override void Encode()
        {
            this.Data.AddVInt(this.Connected);
            this.Data.AddVInt(this.TotalMember);
        }
    }
}
