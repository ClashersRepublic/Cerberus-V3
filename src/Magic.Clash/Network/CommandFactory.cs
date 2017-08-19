using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Network.Commands.Client;

namespace Magic.ClashOfClans.Network
{
    internal static class CommandFactory
    {
        public static readonly Dictionary<int, Type> Commands;

        static CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                { 500, typeof(Buy_Building)},
                { 502, typeof(Upgrade_Building)},
                { 504, typeof(SpeedUp_Construction)},
                { 519, typeof(Mission_Progress) },
                { 591, typeof(Change_Village_Mode)},
            };
        }
    }
}
