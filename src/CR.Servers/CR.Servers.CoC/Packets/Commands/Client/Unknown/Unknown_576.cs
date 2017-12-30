using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_576 : Command
    {
        internal override int Type => 576;

        public Unknown_576(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
