using System;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class UnitUpgradeComponent : Component
    {
        internal override int Type => 9;

        public UnitUpgradeComponent(GameObject GameObject) : base(GameObject)
        {
            // UnitUpgradeComponent.
        }

        internal Timer Timer;
        internal Data UnitData;

        internal int UnitType;

        internal bool UpgradeOnGoing => this.Timer != null;

        internal void CancelUpgrade()
        {
            Player Player = this.Parent.Level.Player;

            if (Player != null)
            {
                if (this.UpgradeOnGoing)
                {
                    if (this.UnitData.GetDataType() == 4)
                    {
                        CharacterData Character = (CharacterData) this.UnitData;

                        int CurrentUpgrade = Player.GetUnitUpgradeLevel(this.UnitData);

                        if (Character.UpgradeCost[CurrentUpgrade] > 0)
                        {
                            Player.Resources.Add(Character.UpgradeResourceData,
                                Character.UpgradeCost[CurrentUpgrade] * 50 / 100);
                        }
                    }
                    else
                    {
                        SpellData Spell = (SpellData) this.UnitData;

                        int CurrentUpgrade = Player.GetUnitUpgradeLevel(this.UnitData);

                        if (Spell.UpgradeCost[CurrentUpgrade] > 0)
                        {
                            Player.Resources.Add(Spell.UpgradeResourceData,
                                Spell.UpgradeCost[CurrentUpgrade] * 50 / 100);
                        }
                    }

                    this.UnitData = null;
                    this.UnitType = 0;
                    this.Timer = null;
                }
            }
        }

        internal bool CanUpgrade(Data Data)
        {
            int DataType = Data.GetDataType();

            return DataType == 4 || DataType == 26;
        }

        internal bool CanStartUpgrading(Data Data)
        {
            Player Player = this.Parent.Level.Player;
            Building Building = (Building) this.Parent;

            if (Player != null)
            {
                if (!this.UpgradeOnGoing)
                {
                    if (this.CanUpgrade(Data))
                    {
                        if (Data.GetDataType() == 4)
                        {
                            CharacterData Character = (CharacterData) Data;

                            if (Character.UnitOfType == 1)
                            {
                                if (!Character.IsUnlockedForBarrackLevel(this.Parent.Level.ComponentManager.MaxBarrackLevel))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (!Character.IsUnlockedForBarrackLevel(this.Parent.Level.ComponentManager.MaxDarkBarrackLevel))
                                {
                                    return false;
                                }
                            }

                            return Building.GetUpgradeLevel() >= Character.LaboratoryLevel[Player.GetUnitUpgradeLevel(Character)] && !Building.Constructing;
                        }

                        SpellData Spell = (SpellData) Data;

                        if (Spell.UnitOfType == 1)
                        {
                            if (!Spell.IsUnlockedForSpellForgeLevel(this.Parent.Level.ComponentManager.MaxBarrackLevel))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!Spell.IsUnlockedForSpellForgeLevel(this.Parent.Level.ComponentManager
                                .MaxDarkBarrackLevel))
                            {
                                return false;
                            }
                        }

                        return Building.GetUpgradeLevel() >= Spell.LaboratoryLevel[Player.GetUnitUpgradeLevel(Spell)] &&  !Building.Constructing;
                    }
                }
            }

            return false;
        }

        internal void FinishUpgrading()
        {
            Player Player = this.Parent.Level.Player;

            if (Player != null)
            {
                if (this.UpgradeOnGoing)
                {
                    Player.IncreaseUnitUpgradeLevel(this.UnitData);

                    this.UnitData = null;
                    this.UnitType = 0;
                    this.Timer = null;
                }
            }
        }

        internal void StartUpgrading(Data Data)
        {
            Player Player = this.Parent.Level.Player;

            if (Player != null)
            {
                if (this.CanStartUpgrading(Data))
                {
                    var Time = Data.GetDataType() == 4
                        ? ((CharacterData) Data).GetUpgradeTime(Player.GetUnitUpgradeLevel(Data))
                        : ((SpellData) Data).GetUpgradeTime(Player.GetUnitUpgradeLevel(Data));

                    this.Timer = new Timer();
                    this.Timer.StartTimer(this.Parent.Level.Player.LastTick, Time);

                    this.UnitData = Data;
                    this.UnitType = Data.GetDataType() > 4 ? 1 : 0;
                }
            }
        }

        internal void SpeedUp()
        {
            if (this.UpgradeOnGoing)
            {
                int RemainingSeconds = this.Timer.GetRemainingSeconds(this.Parent.Level.Player.LastTick);

                if (RemainingSeconds > 0)
                {
                    Player Player = this.Parent.Level.Player;

                    if (Player != null)
                    {
                        int Cost = GamePlayUtil.GetSpeedUpCost(RemainingSeconds, 0, 100);

                        if (Cost > 0)
                        {
                            if (Player.HasEnoughDiamonds(Cost))
                            {
                                Player.UseDiamonds(Cost);
                                this.FinishUpgrading();
                            }
                        }
                    }
                }
            }
        }

        internal override void FastForwardTime(int Secs)
        {
            if (this.UpgradeOnGoing)
            {
                this.Timer.FastForward(Secs);
            }
        }

        internal override void Load(JToken Json)
        {
            JToken UnitUpgrade = Json["unit_upg"];

            if (UnitUpgrade != null)
            {
                if (JsonHelper.GetJsonNumber(UnitUpgrade, "unit_type", out this.UnitType))
                {
                    if (JsonHelper.GetJsonNumber(UnitUpgrade, "id", out int Id) &&
                        JsonHelper.GetJsonNumber(UnitUpgrade, "t", out int Time) &&
                        JsonHelper.GetJsonNumber(UnitUpgrade, "t_end", out int TimeEnd))
                    {
                        this.UnitData = this.UnitType > 0
                            ? CSV.Tables.Get(Gamefile.Spells).GetDataWithID(Id)
                            : CSV.Tables.Get(Gamefile.Characters).GetDataWithID(Id);

                        if (this.UnitData != null)
                        {
                            var startTime = (int) TimeUtils.ToUnixTimestamp(this.Parent.Level.Player.LastTick);
                            int Duration = TimeEnd - startTime;

                            if (Duration < 0)
                            {
                                Duration = 0;
                            }

                            //Duration = Math.Min(Duration, Time);

                            this.Timer = new Timer();
                            this.Timer.StartTimer(this.Parent.Level.Player.LastTick, Duration);
                        }
                    }
                }
            }
        }

        internal override void Save(JObject Json)
        {
            JObject UnitUpgrade = new JObject();

            if (this.UpgradeOnGoing)
            {
                UnitUpgrade.Add("unit_type", this.UnitType);
                UnitUpgrade.Add("id", this.UnitData.GlobalId);
                UnitUpgrade.Add("t", this.Timer.GetRemainingSeconds(this.Parent.Level.Player.LastTick));
                UnitUpgrade.Add("t_end", this.Timer.EndTime);
            }

            Json.Add("unit_upg", UnitUpgrade);
        }

        internal override void Tick()
        {
            if (this.UpgradeOnGoing)
            {
                if (this.Timer.GetRemainingSeconds(this.Parent.Level.Player.LastTick) <= 0)
                {
                    this.FinishUpgrading();
                }
            }
        }
    }
}