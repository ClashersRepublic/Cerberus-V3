using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Boost_Building : Command
    {
        internal override int Type => 526;

        public Boost_Building(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int Count;

        internal override void Decode()
        {
            this.Count = this.Reader.ReadInt32();
            for (int i = 0; i < this.Count; i++)
            {
                this.Reader.ReadInt32();
            }
            base.Decode();
        }
    }
}
