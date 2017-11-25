using System.Collections.Generic;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.CoC.Logic.Clan.Slots;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Clan
{
    internal class Alliance
    {
        [JsonProperty] internal int HighId;
        [JsonProperty] internal int LowId;

        [JsonProperty] internal Members Members;
        //[JsonProperty] internal Streams Streams;
        [JsonProperty] internal AllianceHeader Header;
        [JsonProperty] internal WarState WarState;

        [JsonProperty] internal string Description;

        internal long AllianceId => (long)this.HighId << 32 | (uint)this.LowId;

        internal Alliance()
        {
            this.Header = new AllianceHeader(this);
            this.Members = new Members(this);
            //this.Streams = new Streams(this);
        }

        internal Alliance(int HighID, int LowID) : this()
        {
            this.HighId = HighID;
            this.LowId = LowID;
        }

        internal void Encode(List<byte> Packet)
        {
            this.Header.Encode(Packet);

            Packet.AddString(this.Description);
            Packet.AddInt(0);
            Packet.AddBool(false);
            Packet.AddInt(0);
            Packet.AddBool(false);

            this.Members.Encode(Packet);

            Packet.AddInt(0);
            Packet.AddInt(52);
        }

        public override string ToString()
        {
            return this.HighId + "-" + this.LowId;
        }
    }
}
