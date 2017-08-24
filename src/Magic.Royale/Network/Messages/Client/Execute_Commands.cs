using System;
using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Execute_Commands : Message
    {
        internal int CTick;
        internal uint Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;

        public Execute_Commands(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            CTick = Reader.ReadInt32();
            Checksum = Reader.ReadUInt32();
            Count = Reader.ReadInt32();

            Commands = Reader.ReadBytes((int) (Reader.BaseStream.Length - Reader.BaseStream.Position));
#if DEBUG
            LCommands = new List<Command>(Count);
#endif
        }

        public override void Process()
        {
            //if (!this.Device.Player.Avatar.Modes.IsAttackingOwnBase && this.Device.State == Logic.Enums.State.IN_PC_BATTLE)
            // Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID).Battle_Tick = (int)this.CTick;

            if (Count > -1)
            {
                if (Constants.MaxCommand == 0 || Count <= Constants.MaxCommand)
                    using (var Reader = new Reader(Commands))
                    {
                        for (var _Index = 0; _Index < Count; _Index++)
                        {
                            var CommandID = Reader.ReadInt32();
                            if (CommandFactory.Commands.ContainsKey(CommandID))
                            {
                                var Command = Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader,
                                    Device, CommandID) as Command;

                                if (Command != null)
                                {
#if DEBUG
                                    Logger.Say("Command " + CommandID + " has  been handled.", ConsoleColor.Blue);
#endif
                                    try
                                    {
                                        Command.Decode();
                                    }
                                    catch (Exception Exception)
                                    {
                                        ExceptionLogger.Log(Exception, $"Exception while decoding command {Command.GetType()}");
                                    }

                                    try
                                    {
                                        Command.Process();
                                    }
                                    catch (Exception Exception)
                                    {
                                        ExceptionLogger.Log(Exception, $"Exception while executing command {Command.GetType()}");
                                    }

                                    Device.Last_Command = CommandID;
#if DEBUG
                                    LCommands.Add(Command);
#endif
                                }
                            }
                            else
                            {

                                new Out_Of_Sync(Device).Send();
#if DEBUG
                                Logger.Say("Command " + CommandID + " has not been handled.", ConsoleColor.Red);
                                if (LCommands.Any())
                                    Logger.Say("Previous command was " + LCommands.Last().Identifier + ". [" + (_Index + 1) + " / " + Count + "]", ConsoleColor.Red);
                                break;
#endif
                            }
                        }
                    }
            }
        }
    }
}
