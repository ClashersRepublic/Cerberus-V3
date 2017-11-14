using System.Collections.Generic;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Item
    {
        [JsonProperty] internal Data Data;
        [JsonProperty] internal int Count;

        internal virtual int Checksum => (Data?.GlobalId ?? 0) + this.Count;

        public Item()
        {

        }

        public Item(Data Data, int Count)
        {
            this.Data = Data;
            this.Count = Count;
        }

        internal virtual void Decode(Reader Reader)
        {
            this.Data = Reader.ReadData();
            this.Count = Reader.ReadInt32();
        }

        internal virtual void Encode(List<byte> Packet)
        {
            Packet.AddInt(this.Data.GlobalId);
            Packet.AddInt(this.Count);
        }

        internal virtual void Load(JToken Token)
        {
            JsonHelper.GetJsonData(Token, "id", out this.Data);
            JsonHelper.GetJsonNumber(Token, "cnt", out this.Count);
        }

        internal virtual JObject Save()
        {
            JObject Json = new JObject();

            if (this.Data != null)
            {
                Json.Add("id", this.Data.GlobalId);
            }

            Json.Add("cnt", this.Count);

            return Json;
        }

        public static Item operator +(Item Item, Item Item2)
        {
            if (Item.Data == Item2.Data)
            {
                return new Item(Item.Data, Item.Count + Item2.Count);
            }

            return null;
        }

        public static Item operator -(Item Item, Item Item2)
        {
            if (Item.Data == Item2.Data)
            {
                return new Item(Item.Data, Item.Count + Item2.Count);
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is Item Item)
            {
                return Item.Data == this.Data;
            }

            return false;
        }
    }
}
