using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Speed_Up_Troop_Training : Command
    {
        internal override int Type => 513;

        public Speed_Up_Troop_Training(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal int Manager;

        internal override void Decode()
        {
            this.Manager = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
