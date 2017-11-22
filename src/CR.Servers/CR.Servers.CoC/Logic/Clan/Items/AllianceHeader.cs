using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Clan.Items
{
    internal class AllianceHeader
    {
        internal Alliance Alliance;

        [JsonProperty] internal string Name;

        [JsonProperty] internal int Locale;

        [JsonProperty] internal int Badge;
        [JsonProperty] internal int ExpLevel = 1;
        [JsonProperty] internal int ExpPoints;
        [JsonProperty] internal int Origin;
        [JsonProperty] internal int RequiredScore;
        [JsonProperty] internal int RequiredDuelScore;
        [JsonProperty] internal int NumberOfMembers;

        [JsonProperty] internal int WonWarCount;
        [JsonProperty] internal int LostWarCount;
        [JsonProperty] internal int EqualWarCount;
        [JsonProperty] internal int ConsecutiveWarWinsCount;

        [JsonProperty] internal bool PublicWarLog;
        [JsonProperty] internal bool AmicalWar;

        //[JsonProperty] internal Hiring Type;


    }
}
