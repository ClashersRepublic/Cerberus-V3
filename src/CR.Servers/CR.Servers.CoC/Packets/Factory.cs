namespace CR.Servers.CoC.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Commands.Client;
    using CR.Servers.CoC.Packets.Commands.Client.Battle;
    using CR.Servers.CoC.Packets.Commands.Client.Unknown;
    using CR.Servers.CoC.Packets.Commands.Server;
    using CR.Servers.CoC.Packets.Debugs;
    using CR.Servers.CoC.Packets.Debugs.Elite;
    using CR.Servers.CoC.Packets.Messages.Client.Alliances;
    using CR.Servers.CoC.Packets.Messages.Client.API;
    using CR.Servers.CoC.Packets.Messages.Client.Authentication;
    using CR.Servers.CoC.Packets.Messages.Client.Avatar;
    using CR.Servers.CoC.Packets.Messages.Client.Battle;
    using CR.Servers.CoC.Packets.Messages.Client.Friend;
    using CR.Servers.CoC.Packets.Messages.Client.Home;
    using CR.Servers.CoC.Packets.Messages.Client.Leaderboard;
    using CR.Servers.Extensions.Binary;

    internal static class Factory
    {
        internal const string RC4Key = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";
        internal const char Delimiter = '/';

        internal static readonly Dictionary<short, Type> Messages = new Dictionary<short, Type>();
        internal static readonly Dictionary<int, Type> Commands = new Dictionary<int, Type>();
        internal static readonly Dictionary<string, Type> Debugs = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        internal static void Initialize()
        {
            LoadMessages();
            LoadCommands();
            LoadDebugs();
        }

        private static void LoadMessages()
        {
            Messages.Add(10100, typeof(Client_Hello));
            Messages.Add(10101, typeof(Authentication));
            Messages.Add(10108, typeof(Keep_Alive));
            Messages.Add(10113, typeof(Set_Device_Token));
            Messages.Add(10121, typeof(Unlock_Account));
            Messages.Add(10212, typeof(Change_Avatar_Name));
            Messages.Add(10501, typeof(Accept_Friend_Request));
            Messages.Add(10502, typeof(Add_Friend));
            Messages.Add(10504, typeof(Ask_For_Avatar_Friend_List));
            Messages.Add(10506, typeof(Remove_Friend));
            Messages.Add(10513, typeof(Ask_For_Playing_Facebook_Friend_List));
            Messages.Add(10905, typeof(Inbox_Opened));
            Messages.Add(14101, typeof(Go_Home));
            Messages.Add(14102, typeof(End_Client_Turn));
            Messages.Add(14103, typeof(Cancel_V2_Battle));
            Messages.Add(14113, typeof(Ask_Visit_Home));
            Messages.Add(14134, typeof(Attack_Npc));
            Messages.Add(14201, typeof(Bind_Facebook_Account));
            Messages.Add(14212, typeof(Bind_GameCenter_Account));
            Messages.Add(14301, typeof(Create_Alliance));
            Messages.Add(14302, typeof(Ask_For_Alliance_Data));
            Messages.Add(14303, typeof(Ask_For_Joinable_Alliance_List));
            Messages.Add(14305, typeof(Join_Alliance));
            Messages.Add(14306, typeof(Change_Alliance_Member_Role));
            Messages.Add(14308, typeof(Leave_Alliance));
            Messages.Add(14310, typeof(Donate_Alliance_Unit));
            Messages.Add(14315, typeof(Chat_To_Alliance));
            Messages.Add(14316, typeof(Change_Alliance_Settings));
            Messages.Add(14317, typeof(Request_Join_Alliance));
            Messages.Add(14321, typeof(Respond_To_Alliance_Join_Request));
            Messages.Add(14324, typeof(Search_Alliances));
            Messages.Add(14325, typeof(Ask_For_Avatar_Profile));
            Messages.Add(14334, typeof(Claim_Alliance_Gift));
            Messages.Add(14401, typeof(Request_Alliance_Ranking));
            Messages.Add(14403, typeof(Request_Player_Ranking));
            Messages.Add(14404, typeof(Request_Local_Player_Ranking));
            Messages.Add(14510, typeof(Execute_Battle_Command));
            Messages.Add(14715, typeof(Send_Global_Chat));
            Messages.Add(15110, typeof(Watch_1vs1_Live));
        }

        private static void LoadCommands()
        {
            Commands.Add(1, typeof(Joined_Alliance));
            Commands.Add(2, typeof(Leaved_Alliance));
            Commands.Add(3, typeof(Name_Change_Callback));
            Commands.Add(4, typeof(Donate_Unit_Callback));
            Commands.Add(5, typeof(Alliance_Unit_Received));
            Commands.Add(6, typeof(Changed_Alliance_Settings));
            Commands.Add(7, typeof(Diamonds_Added));
            Commands.Add(8, typeof(Changed_Alliance_Role));

            Commands.Add(500, typeof(Buy_Building)); // ?
            Commands.Add(501, typeof(Move_Building));
            Commands.Add(502, typeof(Upgrade_Building));
            Commands.Add(503, typeof(Sell_Building));
            Commands.Add(504, typeof(Speed_Up_Construction));
            Commands.Add(505, typeof(Cancel_Consturction));
            Commands.Add(506, typeof(Collect_Resource));
            Commands.Add(507, typeof(Clear_Obstacle));
            Commands.Add(508, typeof(Train_Unit));
            Commands.Add(509, typeof(Cancel_Troop_Training));
            Commands.Add(510, typeof(Buy_Trap));
            Commands.Add(511, typeof(Troop_Request));
            Commands.Add(512, typeof(Buy_Deco));
            Commands.Add(513, typeof(Speed_Up_Troop_Training));
            Commands.Add(516, typeof(Upgrade_Unit));
            Commands.Add(517, typeof(Speed_Up_Upgrade_Unit));
            Commands.Add(518, typeof(Buy_Resources));
            Commands.Add(519, typeof(Mission_Progress));
            Commands.Add(520, typeof(Unlock_Building));
            Commands.Add(521, typeof(Free_Worker));
            Commands.Add(522, typeof(Buy_Shield));
            Commands.Add(523, typeof(Claim_Achievement_Reward));
            Commands.Add(524, typeof(Change_Weapon_Mode));
            Commands.Add(526, typeof(Boost_Building));
            Commands.Add(527, typeof(Upgrade_Hero));
            Commands.Add(528, typeof(Speed_Up_Hero_Upgrade));
            Commands.Add(529, typeof(Change_Hero_State));
            Commands.Add(530, typeof(Unknown_530));
            Commands.Add(531, typeof(Cancel_Hero_Upgrade));
            Commands.Add(532, typeof(New_Shop_Seen));
            Commands.Add(533, typeof(Move_Multiple_Buildings));
            Commands.Add(534, typeof(Unknown_534));
            Commands.Add(537, typeof(Send_Alliance_Mail));
            Commands.Add(538, typeof(League_Notifications_Seen));
            Commands.Add(539, typeof(News_Seen));
            Commands.Add(540, typeof(Save_Alliance_Troop_Request_Message));
            Commands.Add(541, typeof(Speed_Up_Troop_Request));
            Commands.Add(543, typeof(Elder_Kick));
            Commands.Add(544, typeof(Edit_Mode_Shown));
            Commands.Add(546, typeof(Move_Building_In_Layout));
            Commands.Add(548, typeof(Finish_Later_Layout));
            Commands.Add(549, typeof(Upgrade_Multiple_Building));
            Commands.Add(550, typeof(Remove_Units));
            Commands.Add(552, typeof(Open_Close_Layout));
            Commands.Add(553, typeof(Unknown_553));
            Commands.Add(554, typeof(Change_Weapon_Heading));
            Commands.Add(556, typeof(Unknown_556));
            Commands.Add(558, typeof(Add_Quick_Train));
            Commands.Add(559, typeof(Train_Quick_Units));
            Commands.Add(560, typeof(Unknown_560));
            Commands.Add(566, typeof(Unknown_566));
            Commands.Add(567, typeof(Set_Active_Layout));
            Commands.Add(568, typeof(Copy_Village_Layout));
            Commands.Add(569, typeof(Remove_All_Building_In_Layout));
            Commands.Add(570, typeof(Unknown_570));
            Commands.Add(571, typeof(Toggle_Clan_Filter));
            Commands.Add(572, typeof(Change_Hero_Mode));
            Commands.Add(573, typeof(Unknown_573));
            Commands.Add(574, typeof(Send_Alliance_Challenge));
            Commands.Add(576, typeof(Unknown_576));
            Commands.Add(577, typeof(Swap_GameObject));
            Commands.Add(579, typeof(Friend_List_Last_Opened));
            Commands.Add(581, typeof(Unknown_581));
            Commands.Add(584, typeof(Boost_Buildings_Of_Type));
            Commands.Add(585, typeof(Unknown_585));
            Commands.Add(586, typeof(Rename_Quick_Train));
            Commands.Add(590, typeof(Buy_Walls));
            Commands.Add(591, typeof(Change_Village_Mode));
            Commands.Add(592, typeof(Train_Unit_V2));
            Commands.Add(593, typeof(Speed_Up_All_Training_V2));
            Commands.Add(595, typeof(Clock_Tower_Boost));
            Commands.Add(596, typeof(Remove_Units_V2));
            Commands.Add(597, typeof(Unknown_597));
            Commands.Add(598, typeof(Layout_Building_Position));
            Commands.Add(599, typeof(Unknown_599));
            Commands.Add(600, typeof(Gear_Up));
            Commands.Add(601, typeof(Search_Opponent_V2));
            Commands.Add(603, typeof(Account_Bound));
            Commands.Add(604, typeof(Seen_Builder_Menu));
            Commands.Add(605, typeof(Unknown_605));
            Commands.Add(700, typeof(Place_Attacker));
            Commands.Add(701, typeof(Place_Alliance_Portal));
            Commands.Add(703, typeof(Surrender_Attack));
            Commands.Add(704, typeof(Place_Spell));
            Commands.Add(705, typeof(Place_Hero));
            Commands.Add(706, typeof(Hero_Rage));
            Commands.Add(711, typeof(Change_Battle_Troop));
            Commands.Add(800, typeof(Search_Opponent));
        }

        private static void LoadDebugs()
        {
            Debugs.Add("addunit", typeof(Add_Unit));
            Debugs.Add("fastforward", typeof(Fast_Forward));
            Debugs.Add("clearobstacle", typeof(Clear_All_Obstacle));
            Debugs.Add("aibase", typeof(AI_Generate_Base));
            Debugs.Add("ailowbase", typeof(AI_Generate_Low_Base));
        }


        internal static Message CreateMessage(short Type, Device Device, Reader Reader)
        {
            if (Messages.TryGetValue(Type, out Type MType))
            {
                return (Message) Activator.CreateInstance(MType, Device, Reader);
            }

            Logging.Error(typeof(Factory), "Can't handle the following message : ID " + Type + ".");

            return null;
        }

        internal static Command CreateCommand(int Type, Device Device, Reader Reader)
        {
            if (Commands.TryGetValue(Type, out Type CType))
            {
                return (Command) Activator.CreateInstance(CType, Device, Reader);
            }

            Logging.Error(typeof(Factory), "Command " + Type + " not exist.");

            return null;
        }

        internal static Debug CreateDebug(string Message, Device Device, out string CommandName)
        {
            string[] Parameters = Message.Remove(0, 1).Split(' ');
            CommandName = Parameters[0];

            if (Debugs.TryGetValue(Parameters[0], out Type DType))
            {
                string[] args = Parameters.Skip(1).ToArray();
                Debug Debug = (Debug) Activator.CreateInstance(DType, Device, args);

                return Debug;
            }
            return null;
        }
    }
}