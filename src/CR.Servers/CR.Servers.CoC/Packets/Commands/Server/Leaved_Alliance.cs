using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Leaved_Alliance : ServerCommand
    {
        internal override int Type => 2;

        public Leaved_Alliance(Device Device) : base(Device)
        {
            
        }

        public Leaved_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal long AllianceId;

        internal override void Decode()
        {
            this.AllianceId = this.Reader.ReadInt64();
            base.Decode();
        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddLong(this.AllianceId);
            base.Encode(Data);
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            Level.Player.AllianceHighId = 0;
            Level.Player.AllianceLowId = 0;
            Level.Player.AllianceMember = null;
            Level.Player.Alliance = null;
        }
    }
}
