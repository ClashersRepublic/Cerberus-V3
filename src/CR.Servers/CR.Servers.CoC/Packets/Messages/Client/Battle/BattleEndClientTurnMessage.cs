#define Extra

namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    using System;
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Battles;
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
            this.Device.GameMode.Time.SubTick = this.SubTick;
            this.Device.GameMode.Level.Tick();

#if Extra
            //Logging.Info(this.GetType(), "Client Time : MS:" + this.Device.GameMode.Time.TotalMS + "  SECS:" + this.Device.GameMode.Time.TotalSecs + ".");
            //Logging.Info(this.GetType(), "Client Subtick : " + this.SubTick + ".");
#endif
            if (this.CommandCount > 0)
            {
                for (int i = 0; i < this.CommandCount; i++)
                {
                    Command Command = this.Commands[i];

                    if (Command.IsServerCommand)
                    {
                        ServerCommand ServerCommand = (ServerCommand)Command;

                        if (this.Device.GameMode.CommandManager.ServerCommands.TryGetValue(ServerCommand.Id, out ServerCommand OriginalCommand))
                        {
                            this.Device.GameMode.CommandManager.ServerCommands.Remove(ServerCommand.Id);
                        }
                        else
                        {
                            this.Reader.BaseStream.Position = 0;
                            this.Log();
                            Logging.Error(this.GetType(), this.Device, "Execute command failed! Server Command " + Command.Type + " is not available.");
                            return;
                        }
                    }

                    if (Command.ExecuteSubTick <= this.SubTick)
                    {
                        try
                        {
                            Command.Execute();
#if Extra
                            Logging.Info(this.GetType(), "Command is handled! (" + Command.Type + ")");
#endif
                        }
                        catch (Exception Exception)
                        {
                            Logging.Error(Exception.GetType(),
                                $"Exception while executing a command {Command.Type}. " + Exception.Message +
                                Environment.NewLine + Exception.StackTrace);
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), this.Device, "Execute command failed! Command should have been executed already. (type=" + Command.Type + ", command_tick=" + Command.ExecuteSubTick + ", server_tick=" + this.SubTick + ")");
                    }
                }
            }

            if (this.Device.Account.DuelBattle != null)
            {
                Battle battle = this.Device.Account.DuelBattle.GetBattle(this.Device.GameMode.Level);

                if (battle != null)
                {
                    if (!battle.Ended)
                    {
                        battle.HandleCommands(this.SubTick, this.Commands);
                    }
                    else
                    {
                        this.Device.Account.DuelBattle = null;
                    }
                }
            }
        }
    }
}