using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic.Structure.Slots.Items
{
    internal class Mail
    {
        [JsonProperty("type")] internal Avatar_Stream Stream_Type;
        [JsonProperty("high_id")] internal int Message_HighID;
        [JsonProperty("low_id")] internal int Message_LowID;

        [JsonProperty("sender_id")] internal long Sender_ID;
        [JsonProperty("sender_name")] internal string Sender_Name;
        [JsonProperty("sender_league")] internal int Sender_League;
        [JsonProperty("sender_lvl")] internal int Sender_Level;
        [JsonProperty("date")] internal DateTime Sent = DateTime.UtcNow;
        [JsonProperty("new")] internal byte New;
        [JsonProperty("alliance_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal long Alliance_ID;

        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal string Message = string.Empty;

        [JsonProperty("battle_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal long Battle_ID;

        [JsonIgnore]
        internal int GetTime => (int) DateTime.UtcNow.Subtract(Sent).TotalSeconds;

        internal byte[] ToBytes
        {
            get
            {
                //Battle Battle = null;
                Clan Clan = null;

                switch (Stream_Type)
                {
                    case Avatar_Stream.ATTACK:
                    case Avatar_Stream.DEFENSE:
                        // Battle = Core.Resources.Battles.Get(Battle_ID, false);
                        break;
                    case Avatar_Stream.REMOVED_CLAN:
                    case Avatar_Stream.CLAN_MAIL:
                    case Avatar_Stream.INVITATION:
                        Clan = ObjectManager.GetAlliance(Alliance_ID);
                        break;
                }


                var _Packet = new List<byte>();
                _Packet.AddInt((int) Stream_Type);
                switch (Stream_Type)
                {
                    /*case Avatar_Stream.ATTACK:
                        _Packet.AddLong(this.Battle_ID);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Battle.Defender.UserId);
                        _Packet.AddString(Battle.Defender.Name);
                        _Packet.AddInt(Battle.Defender.Level);
                        _Packet.AddInt(Battle.Defender.League);
                        break;
                    case Avatar_Stream.DEFENSE:
                        _Packet.AddLong(this.Battle_ID);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Battle.Attacker.UserId);
                        _Packet.AddString(Battle.Attacker.Name);
                        _Packet.AddInt(Battle.Attacker.Level);
                        _Packet.AddInt(Battle.Attacker.League);
                        break;*/
                    default:
                        _Packet.AddInt(Message_HighID);
                        _Packet.AddInt(Message_LowID);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Sender_ID);
                        _Packet.AddString(Sender_Name);
                        _Packet.AddInt(Sender_Level);
                        _Packet.AddInt(Sender_League);
                        break;
                }
                _Packet.AddInt(GetTime);
                _Packet.AddByte(New);
                New = 0;

                switch (Stream_Type)
                {
                    case Avatar_Stream.ATTACK:
                    case Avatar_Stream.DEFENSE:
                        // _Packet.AddString(Battle.Replay_Info.Json);
                        _Packet.AddInt(0);
                        _Packet.AddBool(true);
                        _Packet.AddInt(Convert.ToInt32(Constants.ClientVersion[0]));
                        _Packet.AddInt(Convert.ToInt32(Constants.ClientVersion[1]));
                        _Packet.AddInt(0);

                        _Packet.AddBool(true);
                        _Packet.AddLong(Battle_ID);
                        _Packet.AddInt(int.MaxValue);
                        break;

                    case Avatar_Stream.REMOVED_CLAN:
                        _Packet.AddString(Message);
                        _Packet.AddLong(Alliance_ID);
                        _Packet.AddString(Clan.Name);
                        _Packet.AddInt(Clan.Badge);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Sender_ID);
                        break;

                    case Avatar_Stream.CLAN_MAIL:
                        _Packet.AddString(Message);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Sender_ID);
                        _Packet.AddLong(Alliance_ID);
                        _Packet.AddString(Clan.Name);
                        _Packet.AddInt(Clan.Badge);
                        break;
                    case Avatar_Stream.INVITATION:
                        _Packet.AddLong(Alliance_ID);
                        _Packet.AddString(Clan.Name);
                        _Packet.AddInt(Clan.Badge);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Sender_ID);
                        _Packet.AddInt(1); //Invite id?
                        _Packet.AddByte(0);
                        break;
                }
                return _Packet.ToArray();
            }
        }
    }
}
