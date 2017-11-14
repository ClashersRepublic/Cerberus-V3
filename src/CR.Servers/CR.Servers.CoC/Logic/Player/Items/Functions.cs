using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic
{
    internal class Functions
    {
        [JsonProperty("name")] internal string Name = string.Empty;

        [JsonProperty("parameters")] internal string[] Parameters;
    }
}
