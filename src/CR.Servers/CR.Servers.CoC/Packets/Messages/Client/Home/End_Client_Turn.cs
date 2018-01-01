#define Extra

using System;
using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Core.Consoles.Colorful;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    internal class End_Client_Turn : Message
    {
        internal int SubTick;
        internal int Checksum;
        internal int CommandCount;

        internal List<Command> Commands;

        internal override short Type => 14102;

        public End_Client_Turn(Device Device, Reader Reader) : base(Device, Reader)
        {
            // End_Client_Turn.
        }


        internal override void Decode()
        {
            this.SubTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadInt32();
            this.CommandCount = this.Reader.ReadInt32();
            if (this.CommandCount <= 512)
            {
                if (this.CommandCount > 0)
                {
                    this.Commands = new List<Command>(this.CommandCount);

                    for (int i = 0; i < this.CommandCount; i++)
                    {
                        var CommandID = Reader.ReadInt32();

                        if (Factory.Commands.ContainsKey(CommandID))
                        {
                            var Command = Factory.CreateCommand(CommandID, Device, Reader);

                            if (Command != null)
                            {
                                Command.Decode();

                                this.Commands.Add(Command);
                            }
                            else
                            {
                                Logging.Info(this.GetType(), "Command is null! (" + CommandID + ")");
                                this.CommandCount = this.Commands.Count;
                                break;
                            }
                        }
                        else
                        {
                            this.CommandCount = this.Commands.Count;
                            this.Reader.BaseStream.Position = 0;
                            Logging.Info(this.GetType(), "Command is unhandled! (" + CommandID + ")");
                            Log();
                            break;
                        }
                    }
                }
            }
            else
                Logging.Error(this.GetType(), "Command count is too high! (" + this.CommandCount + ")");

        }

        internal override void Process()
        {
            this.Device.GameMode.Time.SubTick = this.SubTick;
            this.Device.GameMode.Level.Tick();

#if Extra
            //Logging.Info(this.GetType(), "Client Time : MS:" + this.Device.GameMode.Time.TotalMS + "  SECS:" + this.Device.GameMode.Time.TotalSecs + ".");
            //Logging.Info(this.GetType(), "Client Subtick : " + this.SubTick + ".");
#endif
            if (this.CommandCount > 0)
            {
                do
                {
                    //TODO: Tick stuff
                    Command Command = this.Commands[0];

                    if (Command.IsServerCommand)
                    {
                        ServerCommand ServerCommand = (ServerCommand)Command;

                        if (this.Device.GameMode.CommandManager.ServerCommands.TryGetValue(ServerCommand.Id, out ServerCommand OriginalCommand))
                        {
                            /*if (OriginalCommand.Checksum != ServerCommand.Checksum)
                            {
                                return;
                            }*/

                            this.Device.GameMode.CommandManager.ServerCommands.Remove(ServerCommand.Id);
                        }
                        else
                        {
                            this.Reader.BaseStream.Position = 0;
                            Log();
                            Logging.Error(this.GetType(), this.Device, "Execute command failed! Server Command " + Command.Type + " is not available.");
                            return;
                        }
                    }

                    try
                    {
                        Command.Execute();
                    }
                    catch (Exception Exception)
                    {
                        Logging.Error(Exception.GetType(), $"Exception while executing a command {Command.Type}. " + Exception.Message + Environment.NewLine + Exception.StackTrace);
                        this.Reader.BaseStream.Position = 0;
                        Log();
                    }
#if Extra
                    Logging.Info(this.GetType(), "Command is handled! (" + Command.Type + ")");
#endif

                    this.Commands.Remove(Command);
                    continue;
                } while (this.Commands.Count > 0);
            }
        }
    }
}
