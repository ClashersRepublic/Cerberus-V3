using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Client;
using Magic.ClashOfClans.Network.Messages.Client.Authentication;
using Magic.ClashOfClans.Network.Messages.Client.Battle;
using Magic.ClashOfClans.Network.Messages.Client.Clans;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network
{
    internal static class MessageFactory
    {
        private static readonly Dictionary<int, Type> _messages;

        static MessageFactory()
        {
            _messages = new Dictionary<int, Type>
            {
                {10100, typeof(Pre_Authentication)},
                {10101, typeof(Authentication)},
                {10108, typeof(KeepAliveMessage)},
                {10212, typeof(Change_Name)},
                {14101, typeof(Go_Home)},
                {14102, typeof(Execute_Commands)},
                {14113, typeof(Ask_Visit_Home)},
                {14123, typeof(Search_Opponent)},
                {14134, typeof(Attack_NPC)},
                {14301, typeof(Create_Alliance)},
                {14302, typeof(Ask_Alliance) },
                {14315, typeof(Add_Alliance_Message) },
                {14341, typeof(Ask_Bookmark) },
                {14343, typeof(Add_To_Bookmark) },
                {14344, typeof(Remove_From_Bookmark) },
                {14325, typeof(Avatar_Profile)},
                {14715, typeof(Add_Global_Message)},
            };
        }

        public static Message Read(Device device, Reader reader, int messageId)
        {
            if (_messages.ContainsKey(messageId))
                return (Message) Activator.CreateInstance(_messages[messageId], device, reader);

            return null;
        }
    }
}
