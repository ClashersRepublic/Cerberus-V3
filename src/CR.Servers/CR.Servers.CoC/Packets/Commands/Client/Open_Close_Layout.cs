using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Open_Close_Layout : Command
    {
        internal override int Type => 552;

        public Open_Close_Layout(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int Layout;
        internal int State;
        internal bool FinishLater;

        internal override void Decode()
        {
            this.Layout = this.Reader.ReadInt32();
            this.State = this.Reader.ReadInt32();
            this.FinishLater = this.Reader.ReadBoolean();
            base.Decode();
        }
    }
}
