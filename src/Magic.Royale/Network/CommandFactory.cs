using System;
using System.Collections.Generic;
using Magic.Royale.Network.Commands.Client;


namespace Magic.Royale.Network
{
    internal static class CommandFactory
    {
        public static readonly Dictionary<int, Type> Commands;

        static CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {500, typeof(Change_Deck_Card) },
                {501, typeof(Set_Active_Deck) },
                {504, typeof(Upgrade_Card) }
            };
        }
    }
}
