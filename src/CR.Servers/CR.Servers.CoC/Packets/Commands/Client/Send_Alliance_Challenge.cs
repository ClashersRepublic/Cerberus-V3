using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Console.WriteLine(this.Reader.ReadString());
            Console.WriteLine(this.Reader.ReadByte());
            Console.WriteLine(this.Reader.ReadByte());
            base.Decode();
        }
    }
}
