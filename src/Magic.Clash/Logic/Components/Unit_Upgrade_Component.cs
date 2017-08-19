using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Structure;
using Newtonsoft.Json.Linq;
using Resource = Magic.ClashOfClans.Logic.Enums.Resource;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Unit_Upgrade_Component : Component
    {
        internal Combat_Item Unit;
        internal Timer Timer;

        public Unit_Upgrade_Component(Game_Object go) : base(go)
        {
            Timer = null;
            Unit = null;
        }

        public override int Type => 9;
        internal Combat_Item GetUnit => Unit;

        internal bool CanStartUpgrading(Combat_Item data)
        {
            var result = false;
            if (Unit == null)
                if (Parent.ClassId == 0)
                {
                    var b = (Building) Parent;
                    var ca = Parent.Level.Avatar;
                    var cm = Parent.Level.GetComponentManager();
                    var maxProductionBuildingLevel = data.GetCombatItemType() == 1
                        ? cm.GetMaxSpellForgeLevel()
                        : cm.GetMaxBarrackLevel();
                    if (ca.GetUnitUpgradeLevel(data) < data.GetUpgradeLevelCount() - 1)
                        if (maxProductionBuildingLevel >= data.GetRequiredProductionHouseLevel() - 1)
                            result = b.GetUpgradeLevel >=
                                     data.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(data) + 1) - 1;
                }
                else if (Parent.ClassId == 7)
                {
                    var b = (Builder_Building) Parent;
                    var ca = Parent.Level.Avatar;
                    var cm = Parent.Level.GetComponentManager();
                    var maxProductionBuildingLevel = data.GetCombatItemType() == 1
                        ? cm.GetMaxSpellForgeLevel()
                        : cm.GetMaxBarrackLevel();
                    if (ca.GetUnitUpgradeLevel(data) < data.GetUpgradeLevelCount() - 1)
                        if (maxProductionBuildingLevel >= data.GetRequiredProductionHouseLevel() - 1)
                            result = b.GetUpgradeLevel >=
                                     data.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(data) + 1) - 1;
                }
            return result;
        }

        internal void FinishUpgrading()
        {
            if (Unit != null)
            {
                var ca = Parent.Level.Avatar;
                var level = ca.GetUnitUpgradeLevel(Unit);
                ca.SetUnitUpgradeLevel(Unit, level + 1);
            }
            Timer = null;
            Unit = null;
        }

        internal int GetRemainingSeconds()
        {
            var result = 0;
            if (Timer != null)
                result = Timer.GetRemainingSeconds(Parent.Level.Avatar.LastTick);
            return result;
        }

        internal int GetTotalSeconds()
        {
            var result = 0;
            if (Unit != null)
            {
                var ca = Parent.Level.Avatar;
                var level = ca.GetUnitUpgradeLevel(Unit);
                result = Unit.GetUpgradeTime(level);
            }
            return result;
        }

        public override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject) jsonObject["unit_upg"];
            if (unitUpgradeObject != null)
            {
                Timer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                var remainingEndTime = unitUpgradeObject["t_end"].ToObject<int>();

                var startTime = (int) TimeUtils.ToUnixTimestamp(Parent.Level.Avatar.LastTick);
                var duration = remainingEndTime - startTime;

                if (duration < 0)
                    duration = 0;

                Timer.StartTimer(Parent.Level.Avatar.LastTick, duration);

                var id = unitUpgradeObject["id"].ToObject<int>();
                Unit = (Combat_Item) CSV.Tables.GetWithGlobalID(id);
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            if (Unit != null)
            {
                var unitUpgradeObject = new JObject
                {
                    {"unit_type", Unit.GetCombatItemType()},
                    {"t", Timer.GetRemainingSeconds(Parent.Level.Avatar.LastTick)},
                    {"t_end", Timer.EndTime},
                    {"id", Unit.GetGlobalId()}
                };

                jsonObject.Add("unit_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public void SpeedUp()
        {
            if (Unit != null)
            {
                var remainingSeconds = 0;
                if (Timer != null)
                    remainingSeconds = Timer.GetRemainingSeconds(Parent.Level.Avatar.LastTick);
                var cost = GameUtils.GetSpeedUpCost(remainingSeconds);
                var ca = Parent.Level.Avatar;
                if (ca.Resources.Gems >= cost)
                {
                    ca.Resources.Minus(Resource.Diamonds, cost);
                    FinishUpgrading();
                }
            }
        }

        internal void StartUpgrading(Combat_Item cid)
        {
            if (CanStartUpgrading(cid))
            {
                Unit = cid;
                Timer = new Timer();
                Timer.StartTimer(Parent.Level.Avatar.LastTick, GetTotalSeconds());
            }
        }

        public override void Tick()
        {
            if (Timer?.GetRemainingSeconds(Parent.Level.Avatar.LastTick) <= 0)
                FinishUpgrading();
        }
    }
}