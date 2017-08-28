using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic.Structure.Slots.Items
{
    internal class Entry
    {
        [JsonProperty("type")] internal Alliance_Stream Stream_Type = Alliance_Stream.NONE;
        [JsonProperty("high_id")] internal int Message_HighID;
        [JsonProperty("low_id")] internal int Message_LowID;

        [JsonProperty("sender_id")] internal long Sender_ID;
        [JsonProperty("sender_name")] internal string Sender_Name;
        [JsonProperty("sender_role")] internal Role Sender_Role;
        [JsonProperty("sender_league")] internal int Sender_League;
        [JsonProperty("sender_lvl")] internal int Sender_Level;
        [JsonProperty("date")] internal DateTime Sent = DateTime.UtcNow;

        // Stream 1
        [JsonProperty("max_troops", DefaultValueHandling = DefaultValueHandling.Ignore)] internal int Max_Troops;

        [JsonProperty("max_spells", DefaultValueHandling = DefaultValueHandling.Ignore)] internal int Max_Spells;

        [JsonProperty("space_troops", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal int Used_Space_Troops;

        [JsonProperty("space_spells", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal int Used_Space_Spells;

        [JsonProperty("units", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal Castle_Units Units = new Castle_Units();

        [JsonProperty("spells", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal Castle_Units Spells = new Castle_Units();

        [JsonProperty("have_message", DefaultValueHandling = DefaultValueHandling.Ignore)] internal bool Have_Message;

        // Stream 1/2/3
        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal string Message = string.Empty;

        // Stream 3InviteState
        [JsonProperty("judge_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal string Judge_Name = string.Empty;

        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal InviteState Stream_State = InviteState.WAITING;

        // Stream 4
        [JsonProperty("event_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal Events Event_ID = 0;

        [JsonProperty("event_pl_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal long Event_Player_ID;

        [JsonProperty("event_pl_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal string Event_Player_Name = string.Empty;

        // Steam 12
        [JsonProperty("amical_status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal Amical_Mode Amical_State = Amical_Mode.ATTACK;

        [JsonIgnore]
        internal int GetTime => (int) DateTime.UtcNow.Subtract(Sent).TotalSeconds;

        internal void AddTroop(long DonatorId, int Data, int Count, int level)
        {
            var _Index = Units.FindIndex(t => t.Data == Data && t.Player_ID == DonatorId && t.Level == level);
            if (_Index > -1)
            {
                Units[_Index].Count += Count;
            }
            else
            {
                var ds = new Alliance_Unit(DonatorId, Data, Count, level);
                Units.Add(ds);
            }
        }

        internal void AddSpell(long DonatorId, int Data, int Count, int level)
        {
            var _Index = Spells.FindIndex(t => t.Data == Data && t.Player_ID == DonatorId && t.Level == level);
            if (_Index > -1)
            {
                Spells[_Index].Count += Count;
            }
            else
            {
                var ds = new Alliance_Unit(DonatorId, Data, Count, level);
                Spells.Add(ds);
            }
        }


        internal byte[] ToBytes()
        {
            var _Packet = new List<byte>();
            _Packet.AddInt((int) Stream_Type);
            _Packet.AddInt(Message_HighID);
            _Packet.AddInt(Message_LowID);
            _Packet.Add(3);

            _Packet.AddLong(Sender_ID);
            _Packet.AddLong(Sender_ID);
            _Packet.AddString(Sender_Name);

            _Packet.AddInt(Sender_Level);
            _Packet.AddInt(Sender_League);
            _Packet.AddInt((int) Sender_Role);
            _Packet.AddInt(GetTime);

            switch (Stream_Type)
            {
                case Alliance_Stream.TROOP_REQUEST:
                    _Packet.AddInt(Message_LowID);
                    _Packet.AddInt(Max_Troops);
                    _Packet.AddInt(Max_Spells);
                    _Packet.AddInt(Used_Space_Troops);
                    _Packet.AddInt(Used_Space_Spells);
                    _Packet.AddInt(0); // Donator Count

                    _Packet.AddBool(Have_Message);
                    if (Have_Message)
                        _Packet.AddString(Message);

                    _Packet.AddInt(Units.Count + Spells.Count);
                    foreach (var Alliance_Unit in Units)
                    {
                        _Packet.AddInt(Alliance_Unit.Data);
                        _Packet.AddInt(Alliance_Unit.Count);
                        _Packet.AddInt(Alliance_Unit.Level);
                    }
                    foreach (var Alliance_Spell in Spells)
                    {
                        _Packet.AddInt(Alliance_Spell.Data);
                        _Packet.AddInt(Alliance_Spell.Count);
                        _Packet.AddInt(Alliance_Spell.Level);
                    }

                    break;
                case Alliance_Stream.CHAT:
                    _Packet.AddString(Message);
                    break;
                case Alliance_Stream.INVITATION:
                    _Packet.AddString(Message);
                    _Packet.AddString(Judge_Name);
                    _Packet.AddInt((int) Stream_State);
                    break;
                case Alliance_Stream.EVENT:
                    _Packet.AddInt((int) Event_ID);
                    _Packet.AddLong(Event_Player_ID);
                    _Packet.AddString(Event_Player_Name);
                    break;
                case Alliance_Stream.AMICAL_BATTLE:
                    _Packet.AddString(Message);
                    _Packet.AddInt((int) Amical_State);
                    _Packet.AddVInt(1800);
                    break;
            }
            return _Packet.ToArray();
        }
    }
}