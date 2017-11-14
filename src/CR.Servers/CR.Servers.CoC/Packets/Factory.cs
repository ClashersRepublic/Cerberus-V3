using System;
using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Client.Authentication;
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
        }

        private static void LoadMessages()
        {
            Factory.Messages.Add(10101, typeof(Authentication));
            Factory.Messages.Add(10121, typeof(Unlock_Account));
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
    }
}
