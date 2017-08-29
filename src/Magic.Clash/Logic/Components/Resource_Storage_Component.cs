using System.Collections.Generic;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Resource_Storage_Component : Component
    {
        public Resource_Storage_Component(Game_Object go) : base(go)
        {
            CurrentResources = new List<int>();
            MaxResources = new List<int>();
            StolenResources = new List<int>();

            var table = CSV.Tables.Get(Gamefile.Resources);
            var resourceCount = table.Datas.Count;
            for (var i = 0; i < resourceCount; i++)
            {
                CurrentResources.Add(0);
                MaxResources.Add(0);
                StolenResources.Add(0);
            }
        }

        public override int Type => 6;

        internal readonly List<int> CurrentResources;
        internal readonly List<int> StolenResources;
        internal List<int> MaxResources;

        public int GetCount(int resourceIndex)
        {
            return CurrentResources[resourceIndex];
        }

        public int GetMax(int resourceIndex)
        {
            return MaxResources[resourceIndex];
        }

        public void SetMaxArray(List<int> resourceCaps)
        {
            MaxResources = resourceCaps;
            Parent.Level.GetComponentManager().RefreshResourcesCaps();
        }
    }
}
