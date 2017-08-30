using System;
using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Messages.Client.Battle
{
    internal class Execute_Battle_Commands : Message
    {
        internal int CTick;
        internal int Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;


        public Execute_Battle_Commands(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            CTick = Reader.ReadInt32();
            Checksum = Reader.ReadInt32();
            Count = Reader.ReadInt32();

            Commands = Reader.ReadBytes((int) (Reader.BaseStream.Length - Reader.BaseStream.Position));
#if DEBUG
            LCommands = new List<Command>(Count);
#endif
        }

        public override void Process()
        {
            /*if (this.Device.State == Logic.Enums.State.IN_1VS1_BATTLE)
            {
                Resources.Battles_V2.GetPlayer(this.Device.Player.Avatar.Battle_ID_V2, this.Device.Player.Avatar.UserId).Battle_Tick = CTick;
            }*/

            if (Count > -1 && Constants.MaxCommand == 0 || Count > -1 && Count <= Constants.MaxCommand)
            {
                Device.Player.Tick();
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
                                Logger.Say("Battle Command " + CommandID + " has  been handled.", ConsoleColor.Blue);
#endif
                                try
                                {
                                    Command.Decode();
                                }
                                catch (Exception Exception)
                                {
                                    ExceptionLogger.Log(Exception,
                                        $"Exception while decoding command {Command.GetType()}");
                                }

                                try
                                {
                                    Command.Process();
                                }
                                catch (Exception Exception)
                                {
                                    ExceptionLogger.Log(Exception,
                                        $"Exception while executing command {Command.GetType()}");
                                }

                                Device.Last_Command = CommandID;
#if DEBUG
                                LCommands.Add(Command);
#endif
                            }
                        }
                        else
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Battle Command " + CommandID + " has not been handled.");
                            if (LCommands.Any())
                                Console.WriteLine("Previous command was " + LCommands.Last().Identifier + ". [" +
                                                  (_Index + 1) + " / " + Count + "]");
                            Console.ResetColor();
                            break;
#endif
                        }
                    }
                }
            }
        }
    }
}
