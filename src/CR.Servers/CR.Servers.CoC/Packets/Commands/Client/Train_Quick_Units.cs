using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Train_Quick_Units : Command
    {
        internal override int Type => 559;

        public Train_Quick_Units(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int Slot;

        internal override void Decode()
        {
            this.Slot = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
