using System;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic.Battle.Slots;
using CR.Servers.CoC.Logic.Battle.Slots.Items;
using CR.Servers.CoC.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic.Battle
{
    internal class Battle
    {
        internal double Last_Tick;

        internal double Preparation_Time = 30;
        internal double Attack_Time = 180;

        internal double Battle_Tick
        {
            get => this.Preparation_Time > 0 ? this.Preparation_Time : this.Attack_Time;
            set
            {
                if (this.Preparation_Time >= 1 && this.Commands.Count < 1)
                {
                    this.Preparation_Time -= (value - this.Last_Tick) / 63;
                    Console.WriteLine("Preparation Time : " + TimeSpan.FromSeconds(this.Preparation_Time).TotalSeconds);
                }
                else
                {
                    this.Attack_Time -= (value - this.Last_Tick) / 63;
                    Console.WriteLine("Attack Time      : " + TimeSpan.FromSeconds(this.Attack_Time).TotalSeconds);
                }
                this.Last_Tick = value;
                this.End_Tick = (int)value;
            }
        }


        [JsonProperty] internal JObject Level = new JObject();

        [JsonProperty] internal Player Attacker;

        [JsonProperty] internal Player Defender;

        [JsonProperty] internal Replay_Info Replay_Info = new Replay_Info();

        [JsonProperty] internal int End_Tick;

        [JsonProperty] internal int TimeStamp = (int)TimeUtils.ToUnixTimestamp(DateTime.UtcNow);

        [JsonProperty] internal Battle_Command Commands = new Battle_Command();

        [JsonProperty] internal int Preparation_Skip;

        [JsonProperty] internal long Battle_ID;

        internal Battle(long Battle, Level Attacker, Level Enemy)
        {
            this.Battle_ID = Battle;

            this.Attacker = Attacker.Player.Copy();
            this.Defender = Enemy.Player.Copy();
            Enemy.GameObjectManager.Save(this.Level);
        }

        internal Battle()
        {
        }

        internal void Add(Command Command)
        {
            if (this.Preparation_Time > 0)
                Preparation_Skip = (int)System.Math.Round(this.Preparation_Time);

            this.Commands.Add(Command); 
        }

        internal JObject Save()
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
                {"end_tick", this.End_Tick},
                {"timestamp", (int) TimeUtils.ToUnixTimestamp(DateTime.UtcNow)},
                {"cmd", this.Commands.Save()},
                {"calendar",  new JObject()},
                {"globals", Global},
                {"prep_skip", this.Preparation_Skip},
                {"battle_id", this.Battle_ID}
            };
            return Json;
        }

        internal void Set_Replay_Info()
        {
            foreach (var _Slot in this.Defender.Resources)
            {
                this.Replay_Info.Loot.Add(new[] { _Slot.Data, _Slot.Count }); // For Debug
                this.Replay_Info.AvailableLoot.Add(new[] { _Slot.Data, _Slot.Count });
            }

            this.Replay_Info.Stats.HomeId[0] = this.Defender.HighID;
            this.Replay_Info.Stats.HomeId[1] = this.Defender.LowID;
            this.Replay_Info.Stats.OriginalAttackerScore = this.Attacker.Score;
            this.Replay_Info.Stats.OriginalDefenderScore = this.Defender.Score;

            this.Replay_Info.Stats.AllianceName = this.Attacker.Alliance != null ? this.Attacker.Alliance.Header.Name : string.Empty;

            this.Replay_Info.Stats.AllianceBadge = Attacker.Alliance?.Header.Badge ?? 0;
            this.Replay_Info.Stats.BattleTime = 180 - (int)this.Attack_Time + 1;
        }
    }
}
