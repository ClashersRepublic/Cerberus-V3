using System;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;
using Magic.ClashOfClans.Logic.Enums;
using Newtonsoft.Json.Linq;
using Resource = Magic.ClashOfClans.Logic.Enums.Resource;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Construction_Item : Game_Object
    {
        public Construction_Item(Data data, Level level) : base(data, level)
        {
            IsBoosted = false;
            IsBoostPause = false;
            IsConstructing = false;
            IsGearing = false;
            UpgradeLevel = -1;
        }

        internal Timer BoostTimer { get; set; }
        internal Timer Timer { get; set; }

        internal bool Locked { get; set; }
        internal bool IsBoosted { get; set; }
        internal bool IsBoostPause { get; set; }
        internal int UpgradeLevel { get; set; }
        internal bool IsConstructing { get; set; }
        internal bool IsGearing { get; set; }
        internal bool BuilderVillage { get; set; }

        internal DateTime GetBoostEndTime => BoostTimer.GetEndTime;

        internal int GetUpgradeLevel => UpgradeLevel;

        internal bool IsMaxUpgradeLevel => UpgradeLevel >= GetConstructionItemData.GetUpgradeLevelCount() - 1;

        internal bool IsUpgrading => IsConstructing && UpgradeLevel >= 0;

        internal Construction_Item_Data GetConstructionItemData => (Construction_Item_Data) Data;

        internal int GetRemainingConstructionTime => Timer.GetRemainingSeconds(Level.Avatar.LastTick);

        internal void BoostBuilding()
        {
            IsBoosted = true;
            IsBoostPause = false;
            BoostTimer = new Timer();
            BoostTimer.StartTimer(Level.Avatar.LastTick, GetBoostDuration);
        }

        internal int GetBoostDuration => 0;

        internal float GetBoostMultipier => 0;

        internal int GetRequiredTownHallLevelForUpgrade => GetConstructionItemData.GetRequiredTownHallLevel(Math.Min(UpgradeLevel + 1, GetConstructionItemData.GetUpgradeLevelCount() - 1));

        internal bool CanUpgrade
        {
            get
            {
                var result = false;
                if (!IsConstructing)
                    if (UpgradeLevel < GetConstructionItemData.GetUpgradeLevelCount() - 1)
                    {
                        result = true;
                        if (ClassId == 0 || ClassId == 4)
                        {
                            var currentTownHallLevel = Level.Avatar.TownHall_Level;
                            var requiredTownHallLevel = GetRequiredTownHallLevelForUpgrade;
                            if (currentTownHallLevel < requiredTownHallLevel)
                                result = false;
                        }
                        else if ((ClassId == 7) || (ClassId == 11))
                        {
                            var currentBuilderTownHallLevel = Level.Avatar.Builder_TownHall_Level;
                            var requiredTownHallLevel = GetRequiredTownHallLevelForUpgrade;
                            if (currentBuilderTownHallLevel < requiredTownHallLevel)
                                result = false;
                        }
                    }
                return result;
            }
        }

        internal void StartConstructing(int[] xy, bool buildervillage = false, bool instant = false)
        {
            X = xy[0];
            Y = xy[1];
            BuilderVillage = buildervillage;
            var constructionTime = instant ? 0 : GetConstructionItemData.GetConstructionTime(0);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                Timer = new Timer();
                Timer.StartTimer(Level.Avatar.LastTick, constructionTime);
                if (buildervillage)
                    Level.BuilderWorkerManager.AllocateWorker(this);
                else
                    Level.VillageWorkerManager.AllocateWorker(this);
                IsConstructing = true;
            }
        }

        internal void StartUpgrading(bool buildervillage = false)
        {
            BuilderVillage = buildervillage;
            var constructionTime = GetConstructionItemData.GetConstructionTime(UpgradeLevel + 1);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                IsConstructing = true;
                Timer = new Timer();
                Timer.StartTimer(Level.Avatar.LastTick, constructionTime);
                if (buildervillage)
                    Level.BuilderWorkerManager.AllocateWorker(this);
                else
                    Level.VillageWorkerManager.AllocateWorker(this);
            }
        }

        internal void SpeedUpConstruction()
        {
            if (IsConstructing)
            {
                var ca = Level.Avatar;
                var remainingSeconds = Timer.GetRemainingSeconds(Level.Avatar.LastTick);
                var cost = GameUtils.GetSpeedUpCost(remainingSeconds);
                if (ca.Resources.Gems >= cost)
                {
                    ca.Resources.Minus(Resource.Diamonds, cost);
                    FinishConstruction();
                }
            }
        }

        internal void FinishConstruction()
        {
            IsConstructing = false;

            if (BuilderVillage)
                Level.BuilderWorkerManager.DeallocateWorker(this);
            else
                Level.VillageWorkerManager.DeallocateWorker(this);

            SetUpgradeLevel(GetUpgradeLevel + 1);
            /*if (GetResourceProductionComponent() != null)
            {
                GetResourceProductionComponent().Reset();
            }*/

            var constructionTime = GetConstructionItemData.GetConstructionTime(GetUpgradeLevel);
            Level.Avatar.AddExperience((int) Math.Pow(constructionTime, 0.5f));
            if (GetHeroBaseComponent(true) != null)
            {
                Buildings data = (Buildings)Data;
                Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(data.HeroType) as Heroes;
                Console.WriteLine(Level.Avatar.Name);
                Level.Avatar.SetUnitUpgradeLevel(hd, 0);
                Level.Avatar.SetHeroHealth(hd, 0);
                Level.Avatar.SetHeroState(hd, 3);
            }
        }

        internal void CancelConstruction()
        {
            if (IsConstructing)
            {
                var wasUpgrading = IsUpgrading;
                IsConstructing = false;
                if (wasUpgrading)
                    SetUpgradeLevel(UpgradeLevel);
                var bd = GetConstructionItemData;
                var rd = bd.GetBuildResource(UpgradeLevel + 1);
                var cost = bd.GetBuildCost(UpgradeLevel + 1);
                var multiplier = (CSV.Tables.Get(Gamefile.Globals).GetData("BUILD_CANCEL_MULTIPLIER") as Globals)
                    .NumberValue;
                var resourceCount = (int) ((cost * multiplier * (long) 1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                Level.Avatar.Resources.Plus(rd.GetGlobalId(), resourceCount);

                if (BuilderVillage)
                    Level.BuilderWorkerManager.DeallocateWorker(this);
                else
                    Level.VillageWorkerManager.DeallocateWorker(this);

                if (UpgradeLevel == -1)
                    Level.GameObjectManager.RemoveGameObject(this);
            }
        }

        internal void SetUpgradeLevel(int level)
        {
            UpgradeLevel = level;
            if (UpgradeLevel > -1 || IsUpgrading || !IsConstructing)
            {
                /*if (GetUnitStorageComponent(true) != null)
                {
                    var data = (Buildings)GetData();
                    if (data.GetUnitStorageCapacity(level) > 0)
                    {
                        if (!data.Bunker)
                        {
                            GetUnitStorageComponent().SetMaxCapacity(data.GetUnitStorageCapacity(level));
                            GetUnitStorageComponent().SetEnabled(!IsConstructing());
                        }
                    }
                }*/

                /*var resourceStorageComponent = GetResourceStorageComponent(true);
                if (resourceStorageComponent != null)
                {
                    var maxStoredResourcesList = ((Buildings)GetData()).GetMaxStoredResourceCounts(UpgradeLevel);
                    resourceStorageComponent.SetMaxArray(maxStoredResourcesList);
                }*/
            }
        }
        internal Hero_Base_Component GetHeroBaseComponent(bool enabled = false)
        {
            Component comp = GetComponent(10, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Hero_Base_Component)comp;
            }
            return null;
        }

        internal Unit_Upgrade_Component GetUnitUpgradeComponent(bool enabled = false)
        {
            var comp = GetComponent(9, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Unit_Upgrade_Component)comp;
            }
            return null;
        }

        internal Unit_Production_Component GetUnitProductionComponent(bool enabled = false)
        {
            var comp = GetComponent(3, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Unit_Production_Component)comp;
            }
            return null;
        }

        public override void Tick()
        {
            base.Tick();

            if (IsConstructing)
                if (Timer.GetRemainingSeconds(Level.Avatar.LastTick) <= 0)
                    FinishConstruction();
            if (IsBoosted)
                if (BoostTimer.GetRemainingSeconds(Level.Avatar.LastTick) <= 0)
                    IsBoosted = false;

            if (IsGearing)
                if (Timer.GetRemainingSeconds(Level.Avatar.LastTick) <= 0)
                    FinishConstruction();
        }

        public new void Load(JObject jsonObject)
        {
            var builderVillageToken = jsonObject["bv"];
            if (builderVillageToken != null)
                BuilderVillage = builderVillageToken.ToObject<bool>();
            UpgradeLevel = jsonObject["lvl"].ToObject<int>();

            var gearingToken = jsonObject["gearing"];
            var gearingTimeToken = jsonObject["const_t"];
            var gearingTimeEndToken = jsonObject["const_t_end"];

            if (gearingToken != null && gearingTimeToken != null && gearingTimeEndToken != null)
            {
                Timer = new Timer();
                IsGearing = true;

                var remainingGearingTime = gearingTimeEndToken.ToObject<int>();
                var startTime = (int) TimeUtils.ToUnixTimestamp(Level.Avatar.LastTick);
                var duration = remainingGearingTime - startTime;

                if (duration < 0)
                    duration = 0;

                Timer.StartTimer(Level.Avatar.LastTick, duration);


                if (BuilderVillage)
                    Level.BuilderWorkerManager.AllocateWorker(this);
                else
                    Level.VillageWorkerManager.AllocateWorker(this);
            }

            var constTimeToken = jsonObject["const_t"];
            var constTimeEndToken = jsonObject["const_t_end"];
            if (constTimeToken != null && constTimeEndToken != null)
            {
                Timer = new Timer();
                IsConstructing = true;

                var remainingConstructionEndTime = constTimeEndToken.ToObject<int>();
                var startTime = (int) TimeUtils.ToUnixTimestamp(Level.Avatar.LastTick);
                var duration = remainingConstructionEndTime - startTime;

                Timer.StartTimer(Level.Avatar.LastTick, duration > 0 ? duration : 0);

                if (BuilderVillage)
                    Level.BuilderWorkerManager.AllocateWorker(this);
                else
                    Level.VillageWorkerManager.AllocateWorker(this);
            }
            var boostToken = jsonObject["boost_t"];
            var boostEndToken = jsonObject["boost_t_end"];
            if (boostToken != null && boostEndToken != null)
            {
                BoostTimer = new Timer();
                IsBoosted = true;
                //this.IsBoostPause = boostPauseToken.ToObject<bool>();

                var remainingBoostEndTime = boostEndToken.ToObject<int>();
                var startTime = (int) TimeUtils.ToUnixTimestamp(Level.Avatar.LastTick);
                var duration = remainingBoostEndTime - startTime;

                BoostTimer.StartTimer(Level.Avatar.LastTick, duration);
            }
            Locked = false;
            var lockedToken = jsonObject["locked"];
            if (lockedToken != null)
                Locked = lockedToken.ToObject<bool>();
            SetUpgradeLevel(UpgradeLevel);
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            jsonObject.Add("bv", BuilderVillage);
            jsonObject.Add("lvl", UpgradeLevel);
            if (IsGearing)
            {
                jsonObject.Add("gearing", IsGearing);
                jsonObject.Add("const_t", Timer.GetRemainingSeconds(Level.Avatar.LastTick));
                jsonObject.Add("const_t_end", Timer.EndTime);
            }

            if (IsConstructing)
            {
                jsonObject.Add("const_t", Timer.GetRemainingSeconds(Level.Avatar.LastTick));
                jsonObject.Add("const_t_end", Timer.EndTime);
            }
            if (IsBoosted)
            {
                jsonObject.Add("boost_t", BoostTimer.GetRemainingSeconds(Level.Avatar.LastTick));
                jsonObject.Add("boost_t_end", BoostTimer.EndTime);
            }
            if (Locked)
                jsonObject.Add("locked", true);

            base.Save(jsonObject);
            return jsonObject;
        }
    }
}
