using System;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class HeroBaseComponent : Component
    {
        internal override int Type => 10;

        public HeroBaseComponent(GameObject GameObject, HeroData HeroData) : base(GameObject)
        {
            this.HeroData = HeroData;
        }

        internal Timer UpgradeTimer;
        internal HeroData HeroData;

        internal int RemainingUpgradeTime => this.UpgradeTimer?.GetRemainingSeconds(this.Parent.Level.Player.LastTick) ?? 0;
        internal int UpgradeLevel => this.Parent.Level.Player.GetHeroUpgradeLevel(HeroData);
        internal int VillageType => this.HeroData.VillageType;
        internal bool Upgrading => this.UpgradeTimer != null;

        internal bool UpgradeAvailable
        {
            get
            {
                if (!this.Upgrading)
                {
                    var Level = Parent.Level;
                    var Data = this.HeroData;
                    
                    if (Data.MaxLevel > this.UpgradeLevel)
                    {
                        return (this.VillageType == 1 ? Level.GameObjectManager.TownHall2.GetUpgradeLevel() + 1 : Level.GameObjectManager.TownHall.GetUpgradeLevel() + 1) >= Data.RequiredTownHallLevel[this.UpgradeLevel + 1];
                    }
                }
                return false;
            }
        }

        internal void StartUpgrade()
        {
            int Time = this.HeroData.GetUpgradeTime(this.UpgradeLevel);

            if (!this.Upgrading)
            {
                if (this.VillageType == 0)
                {
                    this.Parent.Level.WorkerManager.AllocateWorker(this.Parent);
                }
                else
                {
                    this.Parent.Level.WorkerManagerV2.AllocateWorker(this.Parent);
                }

                this.Parent.Level.Player.HeroStates.Set(this.HeroData, 1);

                if (Time <= 0)
                {
                    this.FinishUpgrading();
                }
                else
                {
                    this.UpgradeTimer = new Timer();
                    this.UpgradeTimer.StartTimer(this.Parent.Level.Player.LastTick, Time);
                }
            }
        }


        internal void SpeedUpUpgrade()
        {
            if (this.Parent.Level.Player != null)
            {
                if (this.Upgrading)
                {
                    int Cost = GamePlayUtil.GetSpeedUpCost(this.RemainingUpgradeTime, this.VillageType, 100);

                    if (this.Parent.Level.Player.HasEnoughDiamonds(Cost))
                    {
                        this.Parent.Level.Player.UseDiamonds(Cost);
                        this.FinishUpgrading();
                    }
                }
            }
        }

        internal void FinishUpgrading()
        {
            if (this.UpgradeLevel + 1 > this.HeroData.MaxLevel)
            {
                Logging.Error(this.GetType(), "Unable to upgrade the hero because the level is out of range! - " + this.HeroData.Name + ".");
                this.Parent.Level.Player.HeroUpgrades.Set(this.HeroData, this.HeroData.MaxLevel);
            }
            else
                this.Parent.Level.Player.IncreaseHeroUpgradeLevel(this.HeroData);

            if (this.VillageType == 0)
            {
                this.Parent.Level.WorkerManager.DeallocateWorker(this.Parent);
            }
            else
            {
                this.Parent.Level.WorkerManagerV2.DeallocateWorker(this.Parent);
            }

            this.Parent.Level.Player.HeroStates.Set(this.HeroData, 3);

            this.UpgradeTimer = null;
        }

        internal override void FastForwardTime(int Seconds)
        {
            if (this.Upgrading)
            {
                this.UpgradeTimer.FastForward(Seconds);
            }

            base.FastForwardTime(Seconds);
        }

        internal override void Tick()
        {
            if (this.Upgrading)
            {
                if (this.UpgradeTimer.GetRemainingSeconds(this.Parent.Level.Player.LastTick) <= 0)
                {
                    this.FinishUpgrading();
                }
            }

            base.Tick();
        }

        internal override void Load(JToken Json)
        {
            JToken HeroUpgrade = Json["hero_upg"];

            if (HeroUpgrade != null)
            {
                if (JsonHelper.GetJsonNumber(HeroUpgrade, "t", out int UpgradeTime) &&
                    JsonHelper.GetJsonNumber(HeroUpgrade, "t_end", out int UpgradeTimeEnd))
                {
                    if (UpgradeTime > -1)
                    {
                        var startTime = (int) TimeUtils.ToUnixTimestamp(this.Parent.Level.Player.LastTick);

                        var duration = UpgradeTimeEnd - startTime;
                        if (duration < 0)
                            duration = 0;

                        //duration = Math.Min(duration, this.HeroData.GetUpgradeTime(this.UpgradeLevel + 1));

                        this.UpgradeTimer = new Timer();
                        this.UpgradeTimer.StartTimer(this.Parent.Level.Player.LastTick, duration);
                    }
                }

                if (JsonHelper.GetJsonNumber(HeroUpgrade, "level", out int Level))
                {
                    if (Level > this.UpgradeLevel + 1)
                    {
                        Logging.Error(this.GetType(), "'Level' seems to be bigger than 'this.UpgradeLevel + 1'");
                    }
                }
            }

            base.Load(Json);
        }

        internal override void Save(JObject Json)
        {
            if (this.Upgrading)
            {
                JObject HeroUpgrade = new JObject
                {
                    {"t", this.UpgradeTimer.GetRemainingSeconds(this.Parent.Level.Player.LastTick)},
                    {"t_end", this.UpgradeTimer.EndTime},
                    {"level", this.UpgradeLevel + 1}
                };

                Json.Add("hero_upg", HeroUpgrade);
            }
        }
    }
}
