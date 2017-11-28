using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Unknown_597 : Command
    {
        internal override int Type => 597;

        public Unknown_597(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int Timestamp;

        internal override void Decode()
        {
            this.Timestamp = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
