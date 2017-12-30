using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_585 : Command
    {
        internal override int Type => 585;

        public Unknown_585(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
