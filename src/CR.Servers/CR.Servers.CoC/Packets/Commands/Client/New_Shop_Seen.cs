using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class New_Shop_Seen : Command
    {
        internal override int Type => 532;

        public New_Shop_Seen(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
