using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Buy_Shield : Command
    {
        internal override int Type => 522;

        public Buy_Shield(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int SheildId;

        internal override void Decode()
        {
            this.SheildId = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
