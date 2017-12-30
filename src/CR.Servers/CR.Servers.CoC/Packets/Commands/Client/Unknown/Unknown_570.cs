using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_570 : Command
    {
        internal override int Type => 570;

        public Unknown_570(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
