using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic.Structure.Slots
{
    internal class DataSlot
    {
        internal Data Data { get; set; }
        internal int Value { get; set; }

        public DataSlot(Data d, int value)
        {
            this.Data = d;
            this.Value = value;
        }

        public void Load(JObject jsonObject)
        {
            this.Data = CSV.Tables.GetWithGlobalID(jsonObject["id"].ToObject<int>());
            this.Value = jsonObject["cnt"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("id", this.Data.GetGlobalId());
            jsonObject.Add("cnt", this.Value);
            return jsonObject;
        }

    }
}
