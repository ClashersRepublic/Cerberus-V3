using System.Text;
using Newtonsoft.Json;

namespace Magic.Royale.Logic.Structure
{
    internal class SCID
    {
        [JsonProperty("High")] internal int High;
        [JsonProperty("Low")] internal int Low;
        [JsonProperty("Value")] internal long Value;

        public SCID() : this(0, 0)
        {
        }

        public SCID(int high, int low)
        {
            High = high;
            Low = low;

            Value = high * 1000000 + low;
        }

        public SCID(long value) : this((int) (value / 1000000), (int) (value % 1000000))
        {
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("SCID(high = ").Append(High)
                .Append(", low = ").Append(Low)
                .Append(", value = ").Append(Value)
                .Append(")").ToString();
        }
    }
}