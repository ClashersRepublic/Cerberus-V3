using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Unit_Production_Component : Component
    {
        internal bool IsSpellForge;

        public Unit_Production_Component(Game_Object go) : base(go)
        {
            SetProductionType(go);
        }

        public override int Type => 3;

        public void SetProductionType(Game_Object go)
        {
            var b = (Construction_Item) Parent;
            var bd = (Buildings) b.Data;
            IsSpellForge = bd.IsSpellForge();
        }

        public override void Load(JObject jsonObject)
        {
            var unitProdObject = (JObject) jsonObject["unit_prod"];
            if (unitProdObject != null)
                IsSpellForge = unitProdObject["unit_type"].ToObject<int>() == 1;
        }

        public override JObject Save(JObject jsonObject)
        {
            var unitProdObject = new JObject {{"m", 1}, {"unit_type", IsSpellForge ? 1 : 0}};
            jsonObject.Add("unit_prod", unitProdObject);
            return jsonObject;
        }
    }
}