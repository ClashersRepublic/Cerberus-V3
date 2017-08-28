using System;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Visit_Home_Data : Message
    {
        internal Level Player;

        internal Visit_Home_Data(Device Device, Level HomeOwner) : base(Device)
        {
            Identifier = 24113;
            Player = HomeOwner;
            Player.Tick();
            this.Device.State = State.VISIT;
        }

        public override void Encode()
        {
            var Home = new Objects(Player, Player.Json);
            Data.AddInt((int) (Home.Timestamp - DateTime.UtcNow).TotalSeconds);

            Data.AddRange(Home.ToBytes);
            Data.AddRange(Player.Avatar.ToBytes);
            Data.AddInt(0);
            Data.Add(1);
            Data.AddRange(Device.Player.Avatar.ToBytes);
        }
    }
}
