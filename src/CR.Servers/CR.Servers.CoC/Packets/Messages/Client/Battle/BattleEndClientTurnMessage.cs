#define Extra

namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class BattleEndClientTurnMessage : Message
    {
        internal int Checksum;
        internal int CommandCount;

        internal List<Command> Commands;
        internal int SubTick;

        public BattleEndClientTurnMessage(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Execute_Battle_Command.
        }

        internal override short Type => 14510;

        internal override void Decode()
        {
            this.SubTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadInt32();
            this.CommandCount = this.Reader.ReadInt32();
            if (this.CommandCount <= 1048)
            {
                if (this.CommandCount > 0)
                {
                    this.Commands = new List<Command>(this.CommandCount);

                    for (int i = 0; i < this.CommandCount; i++)
                    {
                        int CommandID = this.Reader.ReadInt32();

                        if (Factory.Commands.ContainsKey(CommandID))
                        {
                            Command Command = Factory.CreateCommand(CommandID, this.Device, this.Reader);

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
                            this.Log();
                            break;
                        }
                    }
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Command count is too high! (" + this.CommandCount + ")");
            }
        }

        internal override void Process()
        {
            if (this.CommandCount > 0)
            {
                do
                {
                    //TODO: Tick stuff
                    Command Command = this.Commands[0];

                    if (Command.IsServerCommand)
                    {
                        ServerCommand ServerCommand = (ServerCommand) Command;

                        if (this.Device.GameMode.CommandManager.ServerCommands.TryGetValue(ServerCommand.Id, out ServerCommand OriginalCommand))
                        {
                            this.Device.GameMode.CommandManager.ServerCommands.Remove(ServerCommand.Id);
                        }
                        else
                        {
                            this.Reader.BaseStream.Position = 0;
                            this.Log();
                            Logging.Error(this.GetType(), this.Device, "Execute battle command failed! Server Command " + Command.Type + " is not available.");
                            return;
                        }
                    }

                    Command.Execute();
#if Extra
                    Logging.Info(this.GetType(), "Battle Command is handled! (" + Command.Type + ")");
#endif
                    this.Commands.Remove(Command);
                } while (this.Commands.Count > 0);
            }

            this.Device.GameMode.Time.SubTick = this.SubTick;
            this.Device.GameMode.Level.Tick();

            if (this.Device.Account.Battle != null)
            {
                this.Device.Account.Battle.HandleCommands(this.SubTick, this.Commands);
            }
#if Extra
            //Logging.Info(this.GetType(), "Client Time : MS:" + this.Device.GameMode.Time.TotalMS + "  SECS:" + this.Device.GameMode.Time.TotalSecs + ".");
            //Logging.Info(this.GetType(), "Client Subtick : " + this.SubTick + ".");
#endif
        }
    }
}