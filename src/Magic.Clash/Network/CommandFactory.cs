using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Network.Commands.Client;
using Magic.ClashOfClans.Network.Commands.Client.Battle;
using Magic.ClashOfClans.Network.Commands.Client.Clan;
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
                {2, typeof(Leaved_Alliance)},
                {3, typeof(Name_Change_Callback)},
                {6, typeof(Changed_Alliance_Setting)},
                {8, typeof(Role_Update)},
                {500, typeof(Buy_Building)},
                {501, typeof(Move_Building)},
                {502, typeof(Upgrade_Building)},
              //{503, typeof(Sell_Building)},
                {504, typeof(SpeedUp_Construction)},
              //{505, typeof(Cancel_Construction)},
                {506, typeof(Collect_Resources)},
              //{507, typeof(Remove_Obstacle)},
                {508, typeof(Train_Unit)},
              //{509, typeof(Cancel_Unit)},
                {510, typeof(Buy_Trap)},
              //{511, typeof(Request_Alliance_Troops)},
                {512, typeof(Buy_Deco)},
                {513, typeof(SpeedUp_Training)},
              //{514, typeof(SpeedUp_Clearing)},
              //{515, typeof(Cancel_Upgrade_Unit)},
                {516, typeof(Upgrade_Unit)},
                {517, typeof(SpeedUp_Upgrade_Unit)},
                {518, typeof(Buy_Resource)},
                {519, typeof(Mission_Progress)},
                {520, typeof(Unlock_Building)},
                {521, typeof(Free_Worker)},
              //{522, typeof(Buy_Shield)},
                {523, typeof(Claim_Achievement)},
                {524, typeof(Change_Weapon_Mode)},
              //{525, typeof(Load_Turret)},
              //{526, typeof(Boost_Building)},
                {527, typeof(Upgrade_Hero)},
                {528, typeof(SpeedUp_Hero_Upgrade)},
              //{529, typeof(Toggle_Hero_Sleep)},
              //{530, typeof(SpeedUp_Hero_Health)},
              //{531, typeof(Cancel_Hero_Upgrade)},
                {532, typeof(New_Shop_Items_Seen)},
                {533, typeof(Move_Multiple_Buildings)},
              //{534, typeof(Cancel_Shield)},
                {537, typeof(Send_Mail)},
                {538, typeof(My_League)},
              //{539, typeof(News_Seen)},
              //{540, typeof(Request_Alliance_Units)},
              //{541, typeof(SpeedUp_Request_Units)},
              //{543, typeof(Kick_Alliance_Member)},
              //{544, typeof(Get_Village_Layouts)},
              //{546, typeof(Edit_Village_Layout)},
                {549, typeof(Upgrade_Multiple_Buildings)},
                {550, typeof(Remove_Units)},
              //{552, typeof(Save_Village_Layout)},
                {553, typeof(Unknown_553)}, // Client_Server_Tick ?
                {554, typeof(Change_Weapon_Heading)},
              //{558, typeof(Add_Quick_Training_Troop)},
              //{559, typeof(Train_Quick_Units)},
              //{560, typeof(Start_Clan_War)},
              //{567, typeof(Set_Active_Village_Layout)},
              //{568, typeof(Copy_Village_Layout)},
              //{570, typeof(Toggle_Player_War_State)},
              //{571, typeof(Filter_Chat)},
              //{572, typeof(Toggle_Hero_Attack_Mode)},
              //{574, typeof(Request_Amical_Challenge)},
              //{577, typeof(Swap_Buildings)},
              //{584, typeof(Boost_Barracks)},
              //{584, typeof(Rename_Quick_Train)},
                {590, typeof(Buy_Multiple_Wall)},
                {591, typeof(Change_Village_Mode)},
                {592, typeof(Train_Unit_V2)},
                {596, typeof(Remove_Units_V2)},
                {597, typeof(Unknown_597)},
                //{600, typeof(Gear_Up)},
                {604, typeof(Unknown_604)}, // Cast_Spell ?
                //{601, typeof(Search_Opponent_2)},
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
