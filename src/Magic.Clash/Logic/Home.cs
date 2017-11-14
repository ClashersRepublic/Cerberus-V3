using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Files;

namespace Magic.ClashOfClans.Logic
{
    internal class Objects
    {
        internal Level Player;
        internal string Json;

        internal Objects(Level player, string Village)
        {
            Player = player;
            Json = Village;
        }

        internal byte[] ToBytes
        {
            get
            {
                var _Packet = new List<byte>();

                _Packet.AddInt((int) TimeUtils.ToUnixTimestamp(Timestamp));

                _Packet.AddLong(Player.Avatar.UserId);

                _Packet.AddInt(Player.Avatar.Shield);
                _Packet.AddInt(Player.Avatar.Guard);

                _Packet.AddInt((int) TimeSpan.FromDays(365).TotalSeconds); //Personal break

                _Packet.AddCompressed(Json);
                _Packet.AddCompressed(Game_Events.Events_Json);
                _Packet.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
                return _Packet.ToArray();
            }
        }
    }
}
