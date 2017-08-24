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
        }

        public override void Encode()
        {

        }
    }
}
