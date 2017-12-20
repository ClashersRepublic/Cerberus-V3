using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Copy_Village_Layout : Command
    {
        internal override int Type => 568;

        public Copy_Village_Layout(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int From;
        internal int To;

        internal override void Decode()
        {
            base.Decode();
            this.From = this.Reader.ReadInt32();
            this.To = this.Reader.ReadInt32();
        }
    }
}
