using System;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;
using Newtonsoft.Json.Linq;
using Resource = Magic.ClashOfClans.Logic.Enums.Resource;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Hero_Base_Component : Component
    {
        public Hero_Base_Component(Game_Object go, Heroes hd) : base(go)
        {
            HeroData = hd;
        }

        public override int Type => 10;

        internal Heroes HeroData;
        internal Timer Timer;
        internal bool Builder_Village;
        internal int UpgradeLevelInProgress;


        internal int GetRemainingUpgradeSeconds => Timer.GetRemainingSeconds(Parent.Level.Avatar.LastTick);

        internal int GetTotalSeconds => HeroData.GetUpgradeTime(Parent.Level.Avatar.GetUnitUpgradeLevel(HeroData));

        internal bool IsUpgrading => Timer != null;

        internal void CancelUpgrade()
        {
            if (Timer != null)
            {
                var ca = Parent.Level.Avatar;
                var currentLevel = ca.GetUnitUpgradeLevel(HeroData);
                var rd = HeroData.GetUpgradeResource();
                var cost = HeroData.GetUpgradeCost(currentLevel);
                var multiplier = (CSV.Tables.Get(Gamefile.Globals).GetData("HERO_UPGRADE_CANCEL_MULTIPLIER") as Globals)
                    .NumberValue;
                var resourceCount = (int) ((cost * multiplier * (long) 1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                ca.Resources.Plus(rd.GetGlobalId(), resourceCount);
                if (Builder_Village)
                    Parent.Level.BuilderWorkerManager.DeallocateWorker(Parent);
                else
                    Parent.Level.VillageWorkerManager.DeallocateWorker(Parent);
                Timer = null;
            }
        }

        internal bool IsMaxLevel
        {
            get
            {
                var ca = Parent.Level.Avatar;
                var currentLevel = ca.GetUnitUpgradeLevel(HeroData);
                var maxLevel = HeroData.GetUpgradeLevelCount() - 1;
                return currentLevel >= maxLevel;
            }
        }

        internal bool CanStartUpgrading
        {
            get
            {
                var result = false;
                if (Timer == null)
                {
                    var currentLevel = Parent.Level.Avatar.GetUnitUpgradeLevel(HeroData);
                    if (!IsMaxLevel)
                    {
                        var requiredThLevel = HeroData.GetRequiredTownHallLevel(currentLevel + 1);
                        result = Parent.Level.Avatar.TownHall_Level >= requiredThLevel;
                    }
                }
                return result;
            }
        }

        internal void StartUpgrading(bool builder_village)
        {
            if (CanStartUpgrading)
            {
                Builder_Village = builder_village;
                if (builder_village)
                    Parent.Level.BuilderWorkerManager.AllocateWorker(Parent);
                else
                    Parent.Level.VillageWorkerManager.AllocateWorker(Parent);
                Timer = new Timer();
                Timer.StartTimer(Parent.Level.Avatar.LastTick, GetTotalSeconds);
                UpgradeLevelInProgress = Parent.Level.Avatar.GetUnitUpgradeLevel(HeroData) + 1;
            }
        }

        internal void SpeedUpUpgrade()
        {
            var remainingSeconds = 0;
            if (IsUpgrading)
                remainingSeconds = Timer.GetRemainingSeconds(Parent.Level.Avatar.LastTick);
            var cost = GameUtils.GetSpeedUpCost(remainingSeconds);
            var ca = Parent.Level.Avatar;
            if (ca.Resources.Gems >= cost)
            {
                ca.Resources.Minus(Resource.Diamonds, cost);
                FinishUpgrading();
            }
        }

        internal void FinishUpgrading()
        {
            var ca = Parent.Level.Avatar;
            var currentLevel = ca.GetUnitUpgradeLevel(HeroData);
            ca.SetUnitUpgradeLevel(HeroData, currentLevel + 1);
            if (Builder_Village)
                Parent.Level.BuilderWorkerManager.DeallocateWorker(Parent);
            else
                Parent.Level.VillageWorkerManager.DeallocateWorker(Parent);
            Timer = null;
        }

        public override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject) jsonObject["hero_upg"];
            if (unitUpgradeObject != null)
            {
                Timer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                Timer.StartTimer(Parent.Level.Avatar.LastTick, remainingTime);
                UpgradeLevelInProgress = unitUpgradeObject["level"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            if (Timer != null)
            {
                var unitUpgradeObject = new JObject
                {
                    {"level", UpgradeLevelInProgress},
                    {"t", Timer.GetRemainingSeconds(Parent.Level.Avatar.LastTick)}
                };
                jsonObject.Add("hero_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public override void Tick()
        {
            if (this.Timer?.GetRemainingSeconds(Parent.Level.Avatar.LastTick) <= 0)
            {
                FinishUpgrading();
            }
        }
    }
}
