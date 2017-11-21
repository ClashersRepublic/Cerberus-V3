using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.DataStream;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets
{
    internal class ServerCommand : Command
    {
        internal int Id = -1;
        internal override bool IsServerCommand => true;

        internal virtual ChecksumEncoder Checksum => null;
        public ServerCommand(Device Device) :  base(Device)
        {
        }

        public ServerCommand(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.Id = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddInt(this.Id);
            base.Encode(Data);
        }
    }
}
