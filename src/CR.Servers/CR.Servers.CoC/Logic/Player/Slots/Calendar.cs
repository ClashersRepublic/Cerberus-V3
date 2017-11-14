using Newtonsoft.Json;
using System.Collections.Generic;

namespace CR.Servers.CoC.Logic
{
    internal class Calendar
    {
        [JsonProperty("events")] internal List<Event> Events = new List<Event>();
    }
}