using System;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Logic.Battle.Slots;
using CR.Servers.CoC.Logic.Battle.Slots.Items;
using CR.Servers.CoC.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic.Battle
{
    internal class Battle_V2 : IBattle
    {
        internal double LastTick;
        internal bool Finished;

        internal double PreparationTime = 60;
        internal double AttackTime = 180;

        public double BattleTick
        {
            get => this.PreparationTime > 0 ? this.PreparationTime + this.AttackTime : this.AttackTime;
            set
            {
                if (this.PreparationTime >= 1 && this.Commands.Count < 1)
                {
                    this.PreparationTime -= (value - this.LastTick) / 63;
                    //Console.WriteLine($"Preparation Time For {Attacker.LowID} :" + TimeSpan.FromSeconds(this.PreparationTime).TotalSeconds);
                }
                else
                {
                    this.AttackTime -= (value - this.LastTick) / 63;
                    //Console.WriteLine($"Attack Time For {Attacker.LowID} : " + TimeSpan.FromSeconds(this.AttackTime).TotalSeconds);
                }
                this.LastTick = value;
                this.EndTick = (int)value;
            }
        }


        [JsonProperty] internal JObject Level = new JObject();

        [JsonProperty] internal Player Attacker;

        [JsonProperty] internal Player Defender;

        [JsonProperty] internal Replay_Info ReplayInfo = new Replay_Info();

        [JsonProperty] public int EndTick { get; set; }

        [JsonProperty] internal int TimeStamp = (int)TimeUtils.ToUnixTimestamp(DateTime.UtcNow);

        [JsonProperty] public Battle_Command Commands { get; set; }

        [JsonProperty] internal int PreparationSkip;

        internal Battle_V2(Level Attacker, Level Enemy)
        {
            this.Attacker = Attacker.Player.Copy();
            this.Defender = Enemy.Player.Copy();
            this.Level = Enemy.BattleV2();

            this.Commands = new Battle_Command();
        }

        internal Battle_V2()
        {
        }

        public void Add(Command Command)
        {
            if (this.PreparationTime > 0)
                this.PreparationSkip = (int)System.Math.Round(this.PreparationTime);

            this.Commands.Add(Command); 
        }

        public JObject Save()
        {
            var Global = new JObject
            {
                {"Global", new JObject { { "GiftPackExtension", "" } }},
                {"Village1", new JObject()},
                {"Village2", new JObject
                {
                    { "TownHallMaxLevel", 8 },
                    { "ScoreChangeForLosing", new JArray()},
                    { "StrengthRangeForScore", new JArray()}

                }},
                {"KillSwitches", new JObject() }
            };
            

            var Json = new JObject
            {
                {"level", this.Level},
                {"attacker", this.Attacker.Save()},
                {"defender", this.Defender.Save()},
                {"end_tick", this.EndTick},
                {"timestamp", (int) TimeUtils.ToUnixTimestamp(DateTime.UtcNow)},
                {"cmd", this.Commands.Save()},
                {"calendar",  new JObject()},
                {"globals", Global},
                {"prep_skip", this.PreparationSkip}
            };
            return Json;
        }

        internal void Set_Replay_Info()
        {
            foreach (var _Slot in this.Defender.Resources)
            {
                this.ReplayInfo.Loot.Add(new[] { _Slot.Data, _Slot.Count }); // For Debug
                this.ReplayInfo.AvailableLoot.Add(new[] { _Slot.Data, _Slot.Count });
            }

            this.ReplayInfo.Stats.HomeId[0] = this.Defender.HighID;
            this.ReplayInfo.Stats.HomeId[1] = this.Defender.LowID;
            this.ReplayInfo.Stats.OriginalAttackerScore = this.Attacker.Score;
            this.ReplayInfo.Stats.OriginalDefenderScore = this.Defender.Score;

            this.ReplayInfo.Stats.AllianceName = this.Attacker.Alliance != null ? this.Attacker.Alliance.Header.Name : string.Empty;

            this.ReplayInfo.Stats.AllianceBadge = Attacker.Alliance?.Header.Badge ?? 0;
            this.ReplayInfo.Stats.BattleTime = 180 - (int)this.AttackTime + 1;
        }
    }
}
