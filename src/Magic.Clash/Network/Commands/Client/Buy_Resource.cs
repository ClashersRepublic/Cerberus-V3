

using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Buy_Resource : Command
    {
        internal int Resource_Count;
        internal int Resource_Data;
        internal int Gems_Price;
        internal bool EmbedCommands;

        public Buy_Resource(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Resource_Count = Reader.ReadInt32();
            Resource_Data = Reader.ReadInt32();

            EmbedCommands = Reader.ReadBoolean();
            if (EmbedCommands)
            {
                Device.Depth++;
                if (Device.Depth >= MaxEmbeddedDepth)
                {
                    new Out_Of_Sync(Device).Send();
                    return;
                }
            }
            else
            {
                Device.Depth--;
            }
            Reader.ReadInt32();
        }

        public override void Process()
        {
            Gems_Price = GameUtils.GetResourceDiamondCost(Resource_Count, Resource_Data);
            if (Device.Player.Avatar.Resources.Gems >= Gems_Price)
            {
                Device.Player.Avatar.Resources.Minus(Resource.Diamonds, Gems_Price);
                Device.Player.Avatar.Resources.Plus(Resource_Data, Resource_Count);
                if (EmbedCommands)
                {
                    var CommandID = Reader.ReadInt32();
                    if (CommandFactory.Commands.ContainsKey(CommandID))
                    {
                        var Command =
                            Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, Device,
                                CommandID) as Command;

                        if (Command != null)
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Command " + CommandID + " has  been handled.");
                            Console.ResetColor();
#endif
                            Command.Decode();
                            Command.Process();
                        }
                    }
                    else
                    {
                        DatabaseManager.Save(Device.Player);
                        new Out_Of_Sync(Device).Send();
                    }
                }
            }
            else
            {
                new Out_Of_Sync(Device).Send();
            }
        }
    }
}
