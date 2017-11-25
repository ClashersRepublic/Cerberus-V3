using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Commands.Server
{
    internal class Joined_Alliance : ServerCommand
    {
        internal override int Type => 1;

        public Joined_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        public Joined_Alliance(Device Device) : base(Device)
        {

        }

        internal long AllianceId;

        internal string AllianceName;

        internal int AllianceBadge;
        internal int AllianceLevel;

        internal bool CreateAlliance;

        internal override void Decode()
        {
            this.AllianceId = Reader.ReadInt64();
            this.AllianceName = Reader.ReadString();
            this.AllianceBadge = Reader.ReadInt32();
            this.CreateAlliance = Reader.ReadBoolean();
            this.AllianceLevel = Reader.ReadInt32();

            base.Decode();
        }

        internal override void Encode(List<byte> Packet)
        {
            Packet.AddLong(this.AllianceId);
            Packet.AddString(this.AllianceName);
            Packet.AddInt(this.AllianceBadge);
            Packet.AddBool(this.CreateAlliance);
            Packet.AddInt(this.AllianceLevel);

            base.Encode(Packet);
        }
    }
}
