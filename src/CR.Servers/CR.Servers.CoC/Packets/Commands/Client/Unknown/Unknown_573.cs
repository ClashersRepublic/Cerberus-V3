using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_573 : Command
    {
        internal override int Type => 573;

        public Unknown_573(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadByte();
            base.Decode();
        }
    }
}
