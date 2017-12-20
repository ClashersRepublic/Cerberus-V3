using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Move_Building_In_Layout : Command
    {
        internal override int Type => 546;

        public Move_Building_In_Layout(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int X;
        internal int Y;
        internal int BuildingId;
        internal int Layout;

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.BuildingId = this.Reader.ReadInt32();
            this.Layout = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
