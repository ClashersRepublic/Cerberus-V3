using System;
using System.Collections.Generic;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Home
    {
        internal Level Level;

        internal int HighID;
        internal int LowID;

        internal JToken LastSave;

        internal JToken HomeJSON => this.Level != null ? this.Level.Save() : this.LastSave;

        public Home()
        {
            // Home.
        }

        public Home(int HighID, int LowID)
        {
            this.HighID = HighID;
            this.LowID = LowID;
        }

        internal void Encode(List<byte> Packet)
        {
            Packet.AddInt(this.HighID);
            Packet.AddInt(this.LowID);

            Packet.AddInt(0); // Shield
            Packet.AddInt(0); // Guard
            Packet.AddInt(365 * 86400);

            Packet.AddCompressed(this.HomeJSON.ToString());
            Packet.AddCompressed("{}");
            Packet.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
        }

        internal void Load(JToken Json)
        {
            JsonHelper.GetJsonNumber(Json, "home_id_high", out this.HighID);
            JsonHelper.GetJsonNumber(Json, "home_id_low", out this.LowID);
            JsonHelper.GetJsonObject(Json, "level", out this.LastSave);
        }

        internal JObject Save()
        {
            JObject Json = new JObject
            {
                {"home_id_high", this.HighID},
                {"home_id_low", this.LowID},
                {"level", this.HomeJSON}
            };


            return Json;
        }
    }

    internal class HomeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var Home = (Home)value;

            if (Home != null)
            {
                Home.Save().WriteTo(writer);
            }
            else
                LevelFile.StartingHome.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Home Home = (Home)existingValue;

            Home?.Load(JToken.Load(reader));

            return Home;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Home);
        }
    }
}