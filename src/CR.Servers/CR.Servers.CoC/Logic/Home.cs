using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Home
    {

        [JsonProperty("home_id_high")] internal int HighID;
        [JsonProperty("home_id_low")] internal int LowID;
        [JsonProperty("level")] internal JToken LastSave;

        internal Level Level;
        internal DateTime Timestamp = DateTime.UtcNow;
        internal JToken HomeJSON => this.Level != null ? this.Level.Save() : this.LastSave;


        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (this.Level != null)
                LastSave = this.Level.Save();
        }

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
            Packet.AddInt((int)TimeUtils.ToUnixTimestamp(Timestamp));
            Packet.AddInt(this.HighID);
            Packet.AddInt(this.LowID);

            Packet.AddInt(0); // Shield
            Packet.AddInt(0); // Guard
            Packet.AddInt(365 * 86400);

            Packet.AddCompressed(this.HomeJSON.ToString());
            Packet.AddCompressed(Game_Events.Events_Json);
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
}
