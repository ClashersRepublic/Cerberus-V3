using System;
using System.Collections.Generic;
using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Commands.Client;
using CR.Servers.CoC.Packets.Commands.Client.Battle;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Messages.Client.Alliances;
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
        internal static readonly Dictionary<string, Type> Debugs = new Dictionary<string, Type>();

        internal static void Initialize()
        {
            Factory.LoadMessages();
            Factory.LoadCommands();
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
            Factory.Messages.Add(14134, typeof(Attack_Npc));
            Factory.Messages.Add(14301, typeof(Create_Alliance));
            Factory.Messages.Add(14302, typeof(Ask_For_Alliance_Data));
            Factory.Messages.Add(14303, typeof(Ask_For_Joinable_Alliance_List));
            Factory.Messages.Add(14316, typeof(Change_Alliance_Settings));
            Factory.Messages.Add(14324, typeof(Search_Alliances));
            Factory.Messages.Add(14325, typeof(Ask_For_Avatar_Profile));
        }

        private static void LoadCommands()
        {
            Factory.Commands.Add(1, typeof(Joined_Alliance));
            Factory.Commands.Add(3, typeof(Name_Change_Callback));
            Factory.Commands.Add(6, typeof(Changed_Alliance_Settings));
            Factory.Commands.Add(7, typeof(Diamonds_Added));

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
            Factory.Commands.Add(539, typeof(New_Seen));
            Factory.Commands.Add(553, typeof(Unknown_553));
            Factory.Commands.Add(554, typeof(Change_Weapon_Heading));
            Factory.Commands.Add(572, typeof(Change_Hero_Mode));
            Factory.Commands.Add(577, typeof(Swap_GameObject));
            Factory.Commands.Add(591, typeof(Change_Village_Mode));
            Factory.Commands.Add(600, typeof(Gear_Up));
            Factory.Commands.Add(601, typeof(Search_Opponent_V2));
        }


        internal static Message CreateMessage(short Type, Device Device, Reader Reader)
        {
            if (Factory.Messages.TryGetValue(Type, out Type MType))
            {
                return (Message)Activator.CreateInstance(MType, Device, Reader);
            }

            Logging.Info(typeof(Factory), "Can't handle the following message : ID " + Type + ".");

            return null;
        }

        internal static Command CreateCommand(int Type, Device Device, Reader Reader)
        {
            if (Factory.Commands.TryGetValue(Type, out Type CType))
            {
                return (Command)Activator.CreateInstance(CType, Device, Reader);
            }

            Logging.Info(typeof(Factory), "Command " + Type + " not exist.");

            return null;
        }


        internal static Debug CreateDebug(string Message, Device Device)
        {
            string[] Parameters = Message.Remove(0, 1).Split(' ');

            if (Factory.Debugs.TryGetValue(Parameters[0], out Type DType))
            {
                var args = Parameters.Skip(1).ToArray();
                Debug Debug = (Debug)Activator.CreateInstance(DType, Device, args);

                if (Device.GameMode.Level.Player.Rank >= Debug.RequiredRank)
                {
                    return Debug;
                }
                else
                {
                    Debug.SendChatMessage("Debug command failed. Insufficient privileges.");
                }
            }

            return null;
        }
    }
}
