using System;
using System.Collections.Generic;
using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Commands.Client;
using CR.Servers.CoC.Packets.Commands.Client.Battle;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Debugs;
using CR.Servers.CoC.Packets.Debugs.Elite;
using CR.Servers.CoC.Packets.Messages.Client.Alliances;
using CR.Servers.CoC.Packets.Messages.Client.API;
using CR.Servers.CoC.Packets.Messages.Client.Authentication;
using CR.Servers.CoC.Packets.Messages.Client.Avatar;
using CR.Servers.CoC.Packets.Messages.Client.Battle;
using CR.Servers.CoC.Packets.Messages.Client.Home;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets
{
    internal static class Factory
    {
        internal const string RC4Key = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";
        internal const char Delimiter = '/';

        internal static readonly Dictionary<short, Type> Messages = new Dictionary<short, Type>();
        internal static readonly Dictionary<int, Type> Commands = new Dictionary<int, Type>();
        internal static readonly Dictionary<string, Type> Debugs = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        internal static void Initialize()
        {
            Factory.LoadMessages();
            Factory.LoadCommands();
            Factory.LoadDebugs();
        }

        private static void LoadMessages()
        {
            Factory.Messages.Add(10100, typeof(Client_Hello));
            Factory.Messages.Add(10101, typeof(Authentication));
            Factory.Messages.Add(10108, typeof(Keep_Alive));
            Factory.Messages.Add(10121, typeof(Unlock_Account));
            Factory.Messages.Add(10212, typeof(Change_Avatar_Name));
            Factory.Messages.Add(14101, typeof(Go_Home));
            Factory.Messages.Add(14102, typeof(End_Client_Turn));
            Factory.Messages.Add(14103, typeof(Cancel_V2_Battle));
            Factory.Messages.Add(14113, typeof(Ask_Visit_Home));
            Factory.Messages.Add(14134, typeof(Attack_Npc));
            Factory.Messages.Add(14212, typeof(Bind_GameCenter_Account));
            Factory.Messages.Add(14301, typeof(Create_Alliance));
            Factory.Messages.Add(14302, typeof(Ask_For_Alliance_Data));
            Factory.Messages.Add(14303, typeof(Ask_For_Joinable_Alliance_List));
            Factory.Messages.Add(14305, typeof(Join_Alliance));
            Factory.Messages.Add(14308, typeof(Leave_Alliance));
            Factory.Messages.Add(14315, typeof(Chat_To_Alliance));
            Factory.Messages.Add(14316, typeof(Change_Alliance_Settings));
            Factory.Messages.Add(14324, typeof(Search_Alliances));
            Factory.Messages.Add(14325, typeof(Ask_For_Avatar_Profile));
            Factory.Messages.Add(14334, typeof(Claim_Alliance_Gift));
            Factory.Messages.Add(14510, typeof(Execute_Battle_Command));
            Factory.Messages.Add(14715, typeof(Send_Global_Chat));
            Factory.Messages.Add(15110, typeof(Watch_1vs1_Live));
        }

        private static void LoadCommands()
        {
            Factory.Commands.Add(1, typeof(Joined_Alliance));
            Factory.Commands.Add(2, typeof(Leaved_Alliance));
            Factory.Commands.Add(3, typeof(Name_Change_Callback));
            Factory.Commands.Add(6, typeof(Changed_Alliance_Settings));
            Factory.Commands.Add(7, typeof(Diamonds_Added));
            Factory.Commands.Add(8, typeof(Changed_Alliance_Role));

            Factory.Commands.Add(500, typeof(Buy_Building));
            Factory.Commands.Add(501, typeof(Move_Building));
            Factory.Commands.Add(502, typeof(Upgrade_Building));
            Factory.Commands.Add(504, typeof(Speed_Up_Construction));
            Factory.Commands.Add(505, typeof(Cancel_Consturction));
            Factory.Commands.Add(506, typeof(Collect_Resource));
            Factory.Commands.Add(507, typeof(Clear_Obstacle));
            Factory.Commands.Add(508, typeof(Train_Unit));
            Factory.Commands.Add(510, typeof(Buy_Trap));
            Factory.Commands.Add(516, typeof(Upgrade_Unit));
            Factory.Commands.Add(517, typeof(Speed_Up_Upgrade_Unit));
            Factory.Commands.Add(518, typeof(Buy_Resources));
            Factory.Commands.Add(519, typeof(Mission_Progress));
            Factory.Commands.Add(520, typeof(Unlock_Building));
            Factory.Commands.Add(521, typeof(Free_Worker));
            Factory.Commands.Add(524, typeof(Change_Weapon_Mode));
            Factory.Commands.Add(527, typeof(Upgrade_Hero));
            Factory.Commands.Add(528, typeof(SpeedUp_Hero_Upgrade));
            Factory.Commands.Add(529, typeof(Change_Hero_State));
            Factory.Commands.Add(532, typeof(New_Shop_Seen));
            Factory.Commands.Add(533, typeof(Move_Multiple_Buildings));
            Factory.Commands.Add(537, typeof(Send_Alliance_Mail));
            Factory.Commands.Add(539, typeof(New_Seen));
            Factory.Commands.Add(544, typeof(Unknown_544));
            Factory.Commands.Add(549, typeof(Upgrade_Multiple_Building));
            Factory.Commands.Add(553, typeof(Unknown_553));
            Factory.Commands.Add(554, typeof(Change_Weapon_Heading));
            Factory.Commands.Add(572, typeof(Change_Hero_Mode));
            Factory.Commands.Add(577, typeof(Swap_GameObject));
            Factory.Commands.Add(590, typeof(Buy_Walls));
            Factory.Commands.Add(591, typeof(Change_Village_Mode));
            Factory.Commands.Add(592, typeof(Train_Unit_V2));
            Factory.Commands.Add(593, typeof(Speed_Up_All_Training_V2));
            Factory.Commands.Add(596, typeof(Speed_Up_Training_V2));
            Factory.Commands.Add(597, typeof(Unknown_597));
            Factory.Commands.Add(600, typeof(Gear_Up));
            Factory.Commands.Add(601, typeof(Search_Opponent_V2));
            Factory.Commands.Add(700, typeof(Place_Attacker));  
            Factory.Commands.Add(703, typeof(Surrender_Attack));
            Factory.Commands.Add(704, typeof(Place_Spell));
            Factory.Commands.Add(705, typeof(Place_Hero));
            Factory.Commands.Add(706, typeof(Hero_Rage));
            Factory.Commands.Add(800, typeof(Search_Opponent));
        }

        private static void LoadDebugs()
        {
            Factory.Debugs.Add("fastforward", typeof(Fast_Forward));
            Factory.Debugs.Add("clearobstacle", typeof(Clear_All_Obstacle));
        }


        internal static Message CreateMessage(short Type, Device Device, Reader Reader) 
        {
            if (Factory.Messages.TryGetValue(Type, out Type MType))
            {
                return (Message)Activator.CreateInstance(MType, Device, Reader);
            }

            Logging.Error(typeof(Factory), "Can't handle the following message : ID " + Type + ".");

            return null;
        }

        internal static Command CreateCommand(int Type, Device Device, Reader Reader)
        {
            if (Factory.Commands.TryGetValue(Type, out Type CType))
            {
                return (Command)Activator.CreateInstance(CType, Device, Reader);
            }

            Logging.Error(typeof(Factory), "Command " + Type + " not exist.");

            return null;
        }

        internal static Debug CreateDebug(string Message, Device Device, out string CommandName)
        {
            string[] Parameters = Message.Remove(0, 1).Split(' ');
            CommandName = Parameters[0];

            if (Factory.Debugs.TryGetValue(Parameters[0], out Type DType))
            {
                var args = Parameters.Skip(1).ToArray();
                Debug Debug = (Debug) Activator.CreateInstance(DType, Device, args);

                return Debug;
            }
            return null;
        }
    }
}
