using System;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Own_Home_Data : Message
    {
        public Level Avatar;

        public Own_Home_Data(Device device) : base(device)
        {
            Identifier = 24101;
            Device.Player.Tick();
        }

        public override void Encode()
        {
            var Home = new Objects(Avatar = Device.Player, Avatar.Json);

            Data.AddInt((int) (Home.Timestamp - DateTime.UtcNow).TotalSeconds);
            Data.AddInt(-1);

            Data.AddRange(Home.ToBytes);
            Data.AddRange(Avatar.Avatar.ToBytes);
            Data.AddInt(Device.State == State.WAR_EMODE ? 1 : 0);
            Data.AddInt(0);

            Data.AddLong(1462629754000);
            Data.AddLong(1462629754000);
            Data.AddLong(1462629754000);
            Data.AddInt(0);
        }
    }
}
