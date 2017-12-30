using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_560 : Command
    {
        internal override int Type => 560;

        public Unknown_560(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            base.Decode();
            int Count = this.Reader.ReadInt32();

            for (int i = 0; i < Count; i++)
            {
                this.Reader.ReadInt64();
            }
        }
    }
}
