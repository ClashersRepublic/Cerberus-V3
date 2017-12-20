using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Alliance_Unit_Received : ServerCommand
    {
        internal override int Type => 5;

        public Alliance_Unit_Received(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        public Alliance_Unit_Received(Device Device) : base(Device)
        {

        }
        
        internal string Donator;
        internal int UnitType;
        internal int UnitId;
        internal int Level;

        internal override void Encode(List<byte> Data)
        {
            Data.AddString(this.Donator);
            Data.AddInt(this.UnitType);
            Data.AddInt(this.UnitId);
            Data.AddInt(this.Level);
            base.Encode(Data);
        }

        internal override void Decode()
        {
            this.Donator = this.Reader.ReadString();
            this.UnitType = this.Reader.ReadInt32();
            this.UnitId = this.Reader.ReadInt32();
            this.Level = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
