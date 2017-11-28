using System;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class New_Seen : Command
    {
        internal override int Type => 539;

        public New_Seen(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int Unknown;

        internal override void Decode()
        {
            this.Unknown = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            ShowValues();
        }
    }
}
