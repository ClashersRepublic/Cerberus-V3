using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Item
    {
        [JsonProperty("id")] internal int Data;
        [JsonProperty("cnt")] internal int Count;

        internal Data ItemData
        {
            get => _data ?? (_data = CSV.Tables.GetWithGlobalId(this.Data));
            set => _data = value;
        }

        private Data _data;

        internal virtual int Checksum => Data + this.Count;

        public Item()
        {

        }

        public Item(int Data, int Count)
        {
            this.Data = Data;
            this.Count = Count;
        }

        internal virtual void Decode(Reader Reader)
        {
            this.Data = Reader.ReadInt32();
            this.Count = Reader.ReadInt32();
            this.ItemData = CSV.Tables.GetWithGlobalId(this.Data);
        }

        internal virtual void Encode(List<byte> Packet)
        {
            Packet.AddInt(this.Data);
            Packet.AddInt(this.Count);
        }

        internal virtual void Load(JToken Token)
        {
            if (JsonHelper.GetJsonNumber(Token, "id", out this.Data))
                this.ItemData = CSV.Tables.GetWithGlobalId(this.Data);

            JsonHelper.GetJsonNumber(Token, "cnt", out this.Count);
        }

        internal virtual JObject Save()
        {
            JObject Json = new JObject {{"id", this.Data}, {"cnt", this.Count}};
            return Json;
        }

        public static Item operator +(Item Item, Item Item2)
        {
            return Item.Data == Item2.Data ? new Item(Item.Data, Item.Count + Item2.Count) : null;
        }

        public static Item operator -(Item Item, Item Item2)
        {
            return Item.Data == Item2.Data ? new Item(Item.Data, Item.Count + Item2.Count) : null;
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
