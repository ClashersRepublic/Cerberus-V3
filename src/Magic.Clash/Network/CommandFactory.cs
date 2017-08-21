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
                { 501, typeof(Move_Building)},
                { 502, typeof(Upgrade_Building)},
                { 504, typeof(SpeedUp_Construction)},
                { 508, typeof(Train_Unit)},
                { 510, typeof(Buy_Trap)},
                { 512, typeof(Buy_Deco)},
                { 516, typeof(Upgrade_Unit) },
                { 517, typeof(SpeedUp_Upgrade_Unit) },
                { 519, typeof(Mission_Progress) },
                { 520, typeof(Unlock_Building) },
                { 524, typeof(Change_Weapon_Mode)},
                { 527, typeof(Upgrade_Hero)},
                { 528, typeof(SpeedUp_Hero_Upgrade)},
                { 533, typeof(Move_Multiple_Buildings)},
                { 549, typeof(Upgrade_Multiple_Buildings)},
                { 550, typeof(Remove_Units)},
                { 554, typeof(Change_Weapon_Heading)},
                { 590, typeof(Buy_Multiple_Wall)},
                { 591, typeof(Change_Village_Mode)},
                { 700, typeof(Place_Attacker)},
                { 701, typeof(Place_Alliance_Attacker)},
                { 703, typeof(Surrender_Attack)},
                { 704, typeof(Place_Spell)},
                { 705, typeof(Place_Hero)},
                { 706, typeof(Hero_Rage)},
                { 800, typeof(Search_Opponent)},
            };
        }
    }
}
