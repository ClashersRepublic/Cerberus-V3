using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic
{
    internal class Clan
    {
        [JsonProperty("clan_id")] internal long Clan_ID;

        [JsonProperty("r_trophi")] internal int Required_Trophies;

        [JsonProperty("origin")] internal int Origin;
        [JsonProperty("badge")] internal int Badge;

        [JsonProperty("name")] internal string Name = string.Empty;
        [JsonProperty("desc")] internal string Description = string.Empty;

        [JsonProperty("level")] internal int Level = 1;
        [JsonProperty("experience")] internal int Experience;
        [JsonProperty("hide")] internal bool Hide_From_Public;

        [JsonProperty("win")] internal int Won_War;
        [JsonProperty("lost")] internal int Lost_War;
        [JsonProperty("draw")] internal int Draw_War;

        [JsonProperty("war_frequency")] internal int War_Frequency;
        [JsonProperty("war_history")] internal bool War_History;
        [JsonProperty("war_amical")] internal bool War_Amical;

        [JsonProperty("type")] internal Hiring Type = Hiring.OPEN;

        [JsonProperty("members")] internal Members Members;
        [JsonProperty("chats")] internal Chats Chats;

        internal int Donations
        {
            get
            {
                return Members.Values.ToList().TakeWhile(Member => Member != null).Sum(Member => Member.Donations);
            }
        }

        internal int BuilderTrophies
        {
            get
            {
                return Members.Values.ToList().TakeWhile(Member => Member != null).Sum(Member => (Member.Player.Avatar.Builder_Trophies / 2));
            }
        }

        internal int Trophies
        {
            get
            {
                return Members.Values.ToList().TakeWhile(Member => Member != null).Sum(Member => (Member.Player.Avatar.Trophies / 2));
            }
        }

        internal Clan()
        {
            Members = new Members(this);
            Chats = new Chats(this);
        }

        internal Clan(long ClanID = 0)
        {
            Clan_ID = ClanID;

            Members = new Members(this);
            Chats = new Chats(this);
        }

        internal byte[] ToBytes
        {
            get
            {
                var _Packet = new List<byte>();

                _Packet.AddLong(Clan_ID);
                _Packet.AddString(Name);

                _Packet.AddInt(Badge);
                _Packet.AddInt((int) Type);
                _Packet.AddInt(Members.Values.Count);

                 _Packet.AddInt(Trophies);
                _Packet.AddInt(BuilderTrophies);
                _Packet.AddInt(Required_Trophies);

                _Packet.AddInt(Won_War);
                _Packet.AddInt(Lost_War);
                _Packet.AddInt(Draw_War);

                _Packet.AddInt(2000001);
                _Packet.AddInt(War_Frequency);
                _Packet.AddInt(Origin);
                _Packet.AddInt(Experience);
                _Packet.AddInt(Level);

                _Packet.AddInt(0);
                _Packet.AddBool(War_History);
                _Packet.AddInt(0);
                _Packet.AddBool(War_Amical);

                return _Packet.ToArray();
            }
        }

        internal byte[] ToBytesHeader()
        {
            var Packet = new List<byte>();

            Packet.AddLong(Clan_ID);
            Packet.AddString(Name);
            Packet.AddInt(Badge);
            Packet.AddBool(Members.Count <= 1); //Founded a clan bool
            Packet.AddInt(Level);
            Packet.AddInt(1);
            Packet.AddInt(-1);

            return Packet.ToArray();
        }
    }
}
