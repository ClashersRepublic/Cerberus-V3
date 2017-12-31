using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Clock_Tower_Boost : Command
    {
        internal override int Type => 595;

        public Clock_Tower_Boost(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int Id;

        internal override void Decode()
        {
            this.Id = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
