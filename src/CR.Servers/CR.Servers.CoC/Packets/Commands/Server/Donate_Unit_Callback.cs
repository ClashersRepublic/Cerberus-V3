using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Donate_Unit_Callback : ServerCommand
    {
        internal override int Type => 4;

        public Donate_Unit_Callback(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        public Donate_Unit_Callback(Device Device) : base(Device)
        {

        }

        internal long StreamId;
        internal int UnitType;
        internal int UnitId;
        internal bool UseDiamonds;

        internal override void Encode(List<byte> Data)
        {
            Data.AddLong(this.StreamId);
            Data.AddInt(this.UnitType); 
            Data.AddInt(this.UnitId);
            Data.AddBool(this.UseDiamonds); 
            base.Encode(Data);
        }

        internal override void Decode()
        {
            this.StreamId = this.Reader.ReadInt64();
            this.UnitType = this.Reader.ReadInt32();
            this.UnitId = this.Reader.ReadInt32();
            this.UseDiamonds = this.Reader.ReadBoolean();
            base.Decode();
        }
    }
}
