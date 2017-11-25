using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Unknown_553 : Command
    {
        internal override int Type => 553;

        public Unknown_553(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int Unknown1;

        internal override void Decode()
        {
            this.Unknown1 = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
