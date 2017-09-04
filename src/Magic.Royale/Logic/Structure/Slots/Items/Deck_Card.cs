using Newtonsoft.Json;

namespace Magic.Royale.Logic.Structure.Slots.Items
{
    internal class Deck_Card
    {
        [JsonProperty("id")] internal int Index;
        [JsonProperty("SCID")] internal SCID Scid;
    }
}