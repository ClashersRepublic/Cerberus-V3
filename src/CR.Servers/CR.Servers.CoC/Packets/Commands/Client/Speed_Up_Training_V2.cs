using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Speed_Up_Training_V2 : Command
    {
        internal override int Type => 596;

        public Speed_Up_Training_V2(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int BuildingId;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
