using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Finish_Later_Layout : Command
    {
        internal override int Type => 548;

        public Finish_Later_Layout(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int Layout;

        internal override void Decode()
        {
            this.Layout = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
