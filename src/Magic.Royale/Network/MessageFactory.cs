using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Client;
using Magic.ClashOfClans.Network.Messages.Client.Authentication;

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
                {14101, typeof(Go_Home)},
                {14102, typeof(Execute_Commands)},
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
