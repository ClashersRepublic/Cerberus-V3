using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Magic.Royale.Logic.Structure.Slots.Items
{
    internal class Deck_Card
    {
        [JsonProperty("id")] internal int Index;
        [JsonProperty("SCID")] internal SCID Scid;
    }
}
