using Newtonsoft.Json;

namespace Magic.Royale.Logic.Structure.Slots.Items
{
    internal class Card_Item
    {
        [JsonProperty("cnt")] internal int Count;

        [JsonProperty("id")] internal int Index;

        [JsonProperty("SCID")] internal SCID Scid;
        [JsonProperty("lvl")] internal int Level;
        [JsonProperty("status")] internal byte Status;
        [JsonProperty("type")] internal int Type;

        public Card_Item()
        {
        }

        public Card_Item(int _Type, int _ID, int _Count, int _Level, byte _isNew)
        {
            Type = _Type;
            Index = _ID;
            Scid = new SCID(_Type, _ID);
            Count = _Count;
            Level = _Level;
            Status = _isNew;
        }

    }
}
