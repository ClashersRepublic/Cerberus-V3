using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class UnitProductionComponent : Component
    {

        public UnitProductionComponent(GameObject go) : base(go)
        {
            SetProductionType(go);
        }

        internal override int Type => 3;
        internal bool IsSpellForge;

        public void SetProductionType(GameObject go)
        {
            var b = Parent;
            var bd = (BuildingData)b.Data;
            IsSpellForge = bd.IsSpellForge;
        }

        internal override void Load(JToken Json)
        {
            JObject UnitProd = (JObject) Json["unit_prod"];
            if (UnitProd != null)
            {
                if (JsonHelper.GetJsonBoolean(UnitProd, "unit_type", out bool IsSpellForge))
                {
                    this.IsSpellForge = IsSpellForge;
                }
            }
            base.Load(Json);
        }

        internal override void Save(JObject Json)
        {
            var UnitProd = new JObject
            {
                {
                    "m", 1
                },
                {
                    "unit_type", IsSpellForge ? 1 : 0
                }
            };

            Json.Add("unit_prod", UnitProd);
            base.Save(Json);
        }
    }
}