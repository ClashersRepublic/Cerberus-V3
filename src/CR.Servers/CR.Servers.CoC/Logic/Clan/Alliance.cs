using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic.Clan.Items;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Clan
{
    internal class Alliance
    {
        [JsonProperty] internal int HighId;
        [JsonProperty] internal int LowId;

        //[JsonProperty] internal Members Members;
        //[JsonProperty] internal Streams Streams;
        [JsonProperty] internal AllianceHeader Header;

        [JsonProperty] internal string Description;

        internal long AllianceId => (long)this.HighId << 32 | (uint)this.LowId;
    }
}
