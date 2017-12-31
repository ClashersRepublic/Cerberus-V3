using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Account_Bound : Command
    {
        internal override int Type => 603;

        public Account_Bound(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            //TODO: Execute this command
        }
    }
}
