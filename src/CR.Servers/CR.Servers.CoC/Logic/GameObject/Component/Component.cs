using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class Component
    {
        internal GameObject Parent;

        internal virtual int Checksum => 0;

        internal virtual int Type => 0;

        public Component(GameObject GameObject)
        {
            this.Parent = GameObject;
        }

        internal virtual void FastForwardTime(int Secs)
        {

        }

        internal virtual void Load(JToken Json)
        {

        }

        internal virtual void Save(JObject Json)
        {

        }

        internal virtual void Tick()
        {

        }
    }
}