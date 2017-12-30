using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Send_Alliance_Challenge : Command
    {
        internal override int Type => 574;

        public Send_Alliance_Challenge(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadString();
            this.Reader.ReadByte();
            this.Reader.ReadByte();
            base.Decode();
        }
    }
}
