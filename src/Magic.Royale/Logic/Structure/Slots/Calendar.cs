using Magic.Royale.Logic.Structure.Slots.Items;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Calendar
    {
        [JsonProperty("events")] internal List<Event> Events = new List<Event>();
    }
}