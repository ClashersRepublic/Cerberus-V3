using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Helpers;
using UCS.PacketProcessing;

namespace UCS.Packets.Messages.Server
{
    internal class AllianceWarMapDataMessage : Message
    {
        public AllianceWarMapDataMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24335);
        }

        public override void Encode()
        {
            var pl = Client.GetLevel().GetPlayerAvatar();
            var an = ObjectManager.GetAlliance(pl.GetAllianceId());
            var data = new List<byte>();
            data.AddInt32(4); // 4 = Preperation Day; 5 = Battle Day; 6 = End of War
            data.AddInt32(1000); // Time left
            data.AddInt64(an.GetAllianceId()); // Alliance ID
            data.AddString(an.GetAllianceName()); // Alliance Name
            data.AddInt32(an.GetAllianceBadgeData()); // Alliance Badge Data
            data.AddInt32(an.GetAllianceLevel()); // Alliance Level
            data.AddInt32(5); // War Members

            #region Player 1 Own Clan
            data.AddInt64(pl.GetAllianceId()); // Alliance ID
            data.AddInt64(pl.GetId()); // Player ID
            data.AddInt64(pl.GetCurrentHomeId()); // Current Home ID
            data.AddString(pl.GetAvatarName()); // Player 1 Name // or pl.GetAvatarName()
            data.AddInt32(0); //StarGivenUp
            data.AddInt32(0); //Damage
            data.AddInt32(0); //Unknown 1
            data.AddInt32(0); //AttackUsed
            data.AddInt32(0); //TotalDefence
            data.AddInt32(3); //Gold Gain
            data.AddInt32(3); //Elixir Gain
            data.AddInt32(3); //DElixir Gain
            data.AddInt32(101000); //Gold Available
            data.AddInt32(101000); //Elixir Available
            data.AddInt32(550); //DElixir Available
            data.AddInt32(0); //OffencesWeight
            data.AddInt32(0); //DefencesWeight
            data.AddInt32(0); //Unknown2
            data.AddInt32(pl.GetTownHallLevel()); //TownHall Level
            data.AddInt32(1); // Map Position
            #endregion Player 1 Own Clan

            // TODO: Other Players & Other Clan

            Encrypt(data.ToArray());
        }
    }
}
