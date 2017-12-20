using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Cancel_Troop_Training : Command
    {
        internal override int Type => 509;

        public Cancel_Troop_Training(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal int UnitId;
        internal int UnitCount;

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.UnitId = this.Reader.ReadInt32();
            this.UnitCount = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
