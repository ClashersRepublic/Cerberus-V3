using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Magic.ClashOfClans.Logic.Structure.Slots
{
    internal class Calendar
    {
        [JsonProperty("events")] internal List<Event> Events = new List<Event>();
    }
}