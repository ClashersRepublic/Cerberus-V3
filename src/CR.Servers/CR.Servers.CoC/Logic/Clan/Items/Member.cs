using System;
using System.Runtime.Serialization;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic.Enums;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Logic.Clan.Items
{
    internal class Member
    {
        [JsonProperty] internal int HighId;
        [JsonProperty] internal int LowId;

        [JsonProperty] internal int TroopReceived;
        [JsonProperty] internal int TroopSended;

        [JsonProperty] internal Role Role;
        [JsonProperty] internal DateTime Joined;

        private Player _Player;

        internal Member()
        {       
        }

        internal Member(Player Player)
        {
            this.HighId = Player.HighID;
            this.LowId = Player.LowID;
            this.Player = Player;

            this.Joined = DateTime.UtcNow;
            this.Role = Role.Member;
        }

        internal long PlayerId => (long) this.HighId << 32 | (uint) this.LowId;
        internal int New => Joined >= DateTime.UtcNow.AddDays(-3) ? 1 : 0;

        internal Player Player
        {
            get
            {
                if (_Player != null)
                    return _Player;

                _Player = Resources.Accounts.LoadAccount(this.HighId, this.LowId)?.Player;

                if (_Player == null)
                    Logging.Error(this.GetType(), $"LoadLevel returned null for Player with account id {HighId} - {LowId}");

                return _Player;
            }
            set => _Player = value;
        }
    }
}
