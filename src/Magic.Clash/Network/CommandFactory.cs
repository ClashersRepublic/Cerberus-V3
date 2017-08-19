using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Network.Commands.Client;
using Magic.ClashOfClans.Network.Commands.Client.Battle;

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
                { 510, typeof(Buy_Trap)},
                { 519, typeof(Mission_Progress) },
                { 590, typeof(Buy_Multiple_Wall)},
                { 591, typeof(Change_Village_Mode)},
                { 700, typeof(Place_Attacker)},
            };
        }
    }
}
