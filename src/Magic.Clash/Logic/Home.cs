using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans;
using System;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Logic
{
    internal class Objects
    {
        internal Level Player;
        internal string Json;
        internal DateTime Timestamp = DateTime.UtcNow;

        internal Objects(Level player, String Village)
        {
            this.Player = player;
            this.Json = Village;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddInt((int)TimeUtils.ToUnixTimestamp(Timestamp));

                _Packet.AddLong(Player.Avatar.UserId);

                _Packet.AddInt(Player.Avatar.Shield);
                _Packet.AddInt(Player.Avatar.Guard);

                _Packet.AddInt((int)TimeSpan.FromDays(365).TotalSeconds); //Personal break

                _Packet.AddCompressed(this.Json);
                _Packet.AddCompressed(Game_Events.Events_Json);
                _Packet.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
                return _Packet.ToArray();
            }
        }
    }
}
