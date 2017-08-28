using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Network.Commands.Client;
using Magic.ClashOfClans.Network.Commands.Client.Battle;
using Magic.ClashOfClans.Network.Commands.Server;

namespace Magic.ClashOfClans.Network
{
    internal static class CommandFactory
    {
        public static readonly Dictionary<int, Type> Commands;

        static CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {1, typeof(Joined_Alliance)},
                {3, typeof(Name_Change_Callback)},
                {8, typeof(Role_Update)},
                {500, typeof(Buy_Building)},
                {501, typeof(Move_Building)},
                {502, typeof(Upgrade_Building)},
                {504, typeof(SpeedUp_Construction)},
                {508, typeof(Train_Unit)},
                {510, typeof(Buy_Trap)},
                {512, typeof(Buy_Deco)},
                {513, typeof(SpeedUp_Training)},
                {516, typeof(Upgrade_Unit)},
                {517, typeof(SpeedUp_Upgrade_Unit)},
                {518, typeof(Buy_Resource)},
                {519, typeof(Mission_Progress)},
                {520, typeof(Unlock_Building)},
                {521, typeof(Free_Worker)},
                {523, typeof(Claim_Achievement)},
                {524, typeof(Change_Weapon_Mode)},
                {527, typeof(Upgrade_Hero)},
                {528, typeof(SpeedUp_Hero_Upgrade)},
                {532, typeof(New_Shop_Items_Seen)},
                {533, typeof(Move_Multiple_Buildings)},
                {538, typeof(My_League)},
                {549, typeof(Upgrade_Multiple_Buildings)},
                {550, typeof(Remove_Units)},
                {553, typeof(Unknown_553)},
                {554, typeof(Change_Weapon_Heading)},
                {590, typeof(Buy_Multiple_Wall)},
                {591, typeof(Change_Village_Mode)},
                {597, typeof(Unknown_597)},
                {604, typeof(Unknown_604)},
                {700, typeof(Place_Attacker)},
                {701, typeof(Place_Alliance_Attacker)},
                {703, typeof(Surrender_Attack)},
                {704, typeof(Place_Spell)},
                {705, typeof(Place_Hero)},
                {706, typeof(Hero_Rage)},
                {709, typeof(Trap_Triggered)},
                {800, typeof(Search_Opponent)},
            };
        }
    }
}
