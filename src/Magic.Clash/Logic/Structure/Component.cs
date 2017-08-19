using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Component
    {
        public Component()
        {
            // Space
        }

        public Component(Game_Object parent)
        {
            Parent = parent;
        }

        public virtual int Type => -1;

        public Game_Object Parent { get; }

        public bool IsEnabled { get; set; }

        public virtual void Load(JObject jsonObject)
        {
            // Space
        }

        public virtual JObject Save(JObject jsonObject)
        {
            return jsonObject;
        }

        public virtual void Tick()
        {
            // Space
        }
    }
}