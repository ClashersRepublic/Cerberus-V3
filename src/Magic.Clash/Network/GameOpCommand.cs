using Magic.ClashOfClans;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;
using System;

namespace Magic.ClashOfClans.Network
{
    internal class GameOpCommand
    {
        public static void SendCommandFailedMessage(Device c)
        {

        }

        public virtual void Execute(Level level)
        {
            // Space
        }

        public byte RequiredPrivileges { get; set; }
    }
}