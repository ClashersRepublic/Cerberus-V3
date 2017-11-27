﻿using System.Collections.Generic;
using CR.Servers.Extensions.List;
using CR.Servers.Logic.Enums;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Clan.Items
{
    internal class AllianceHeader
    {
        internal Alliance Alliance;

        [JsonProperty] internal string Name;

        [JsonProperty] internal int Locale;

        [JsonProperty] internal int Badge;
        [JsonProperty] internal int ExpLevel = 1;
        [JsonProperty] internal int ExpPoints;
        [JsonProperty] internal int Origin;
        [JsonProperty] internal int WarFrequency;
        [JsonProperty] internal int RequiredScore;
        [JsonProperty] internal int RequiredDuelScore;
        [JsonProperty] internal int NumberOfMembers;

        [JsonProperty] internal int WonWarCount;
        [JsonProperty] internal int LostWarCount;
        [JsonProperty] internal int EqualWarCount;
        [JsonProperty] internal int ConsecutiveWarWinsCount;

        [JsonProperty] internal bool PublicWarLog;
        [JsonProperty] internal bool AmicalWar;

        [JsonProperty] internal Hiring Type;

        internal int TypeinInt => (int)this.Type;

        internal int DuelScore
        {
            get
            {
                int Score = 0;

                foreach (Member Member in this.Alliance.Members.Slots.Values)
                {
                    if (Member.Player != null)
                    {
                        Score += Member.Player.DuelScore;
                    }
                    else
                    {
                        this.Alliance.Members.Quit(Member);
                    }
                }

                return Score;
            }
        }

        internal int Score
        {
            get
            {
                int Score = 0;

                foreach (Member Member in this.Alliance.Members.Slots.Values)
                {
                    if (Member.Player != null)
                    {
                        Score += Member.Player.Score;
                    }
                    else
                    {
                        this.Alliance.Members.Quit(Member);
                    }
                }

                return Score;
            }
        }

        public AllianceHeader(Alliance Alliance)
        {
            this.Alliance = Alliance;
        }


        internal void Encode(List<byte> Packet)
        {
            Packet.AddLong(this.Alliance.AllianceId);

            Packet.AddString(this.Name);

            Packet.AddInt(this.Badge);
            Packet.AddInt((int)this.Type);
            Packet.AddInt(this.NumberOfMembers);
            Packet.AddInt(this.Score);
            Packet.AddInt(this.DuelScore);
            
            Packet.AddInt(this.RequiredScore);
            Packet.AddInt(this.RequiredDuelScore);

            Packet.AddInt(this.WonWarCount);
            Packet.AddInt(this.LostWarCount);
            Packet.AddInt(this.EqualWarCount);

            Packet.AddInt(this.Locale);
            Packet.AddInt(this.WarFrequency);
            Packet.AddInt(this.Origin);

            Packet.AddInt(this.ExpPoints);
            Packet.AddInt(this.ExpLevel);
            Packet.AddInt(this.ConsecutiveWarWinsCount);
            Packet.AddBool(this.PublicWarLog);
            Packet.AddInt(0);
            Packet.AddBool(this.AmicalWar);
        }
    }
}