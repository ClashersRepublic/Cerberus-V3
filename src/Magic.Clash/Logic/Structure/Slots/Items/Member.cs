using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Logic.Enums;
using Newtonsoft.Json;

namespace Magic.ClashOfClans.Logic.Structure.Slots.Items
{
    internal class Member
    {
        [JsonProperty("user_id")] internal long UserID;

        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;
        [JsonProperty("role")] internal Role Role = Role.Member;

        [JsonProperty("joined")] internal DateTime Joined = DateTime.UtcNow;

        internal bool Connected => ResourcesManager.IsPlayerOnline(Player);
        internal Level Player => ResourcesManager.GetPlayer(UserID, true);
        internal int New => Joined >= DateTime.UtcNow.AddDays(-3) ? 1 : 0;

        internal Member()
        {
        }

        internal Member(Avatar Player)
        {
            UserID = Player.UserId;

            Joined = DateTime.UtcNow;
            Role = Role.Member;
        }
    }
}
