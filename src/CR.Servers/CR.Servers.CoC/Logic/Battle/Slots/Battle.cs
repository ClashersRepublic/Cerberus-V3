using System;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Logic.Battle.Slots;
using CR.Servers.CoC.Logic.Battle.Slots.Items;
using CR.Servers.CoC.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic.Battle
{
    internal class Battle : IBattle
    {
        internal double LastTick;

        internal double PreparationTime = 30;
        internal double AttackTime = 180;

        public double BattleTick
        {
            get => this.PreparationTime > 0 ? this.PreparationTime : this.AttackTime;
            set
            {
                if (this.PreparationTime >= 1 && this.Commands.Count < 1)
                {
                    this.PreparationTime -= (value - this.LastTick) / 63;
                    Console.WriteLine("Preparation Time : " + TimeSpan.FromSeconds(this.PreparationTime).TotalSeconds);
                }
                else
                {
                    this.AttackTime -= (value - this.LastTick) / 63;
                    Console.WriteLine("Attack Time      : " + TimeSpan.FromSeconds(this.AttackTime).TotalSeconds);
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

        [JsonProperty] internal long BattleId;

        internal Battle(long battle, Level attacker, Level enemy)
        {
            this.BattleId = battle;

            this.Attacker = attacker.Player.Copy();
            this.Defender = enemy.Player.Copy();
            this.Level = enemy.Battle();

            this.Commands = new Battle_Command();
        }

        internal Battle()
        {
        }

        public void Add(Command Command)
        {
            if (this.PreparationTime > 0)
                PreparationSkip = (int)System.Math.Round(this.PreparationTime);
            Console.WriteLine(Command.Save());
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
                    { "TownHallMaxLevel", 6 },
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
                {"prep_skip", this.PreparationSkip},
                {"battle_id", this.BattleId}
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
