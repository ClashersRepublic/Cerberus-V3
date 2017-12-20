using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Remove_All_Building_In_Layout : Command
    {
        internal override int Type => 569;

        public Remove_All_Building_In_Layout(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            base.Decode();
            this.Reader.ReadInt32();
            this.Reader.ReadByte();
        }
    }
}
