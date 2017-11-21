using System.Collections.Generic;
using System.Threading;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Packets;
using CR.Servers.CoC.Packets.Messages.Server.Home;

namespace CR.Servers.CoC.Logic.Manager
{
    internal class CommandManager
    {
        internal bool ChangeNameOnGoing;
        internal int NextServerCommandId;

        internal Level Level;
        internal Dictionary<int, ServerCommand> ServerCommands;

        public CommandManager(Level Level)
        {
            this.Level = Level;
            this.ServerCommands = new Dictionary<int, ServerCommand>();
        }

        internal void AddCommand(ServerCommand Command)
        {
            if (Command.IsServerCommand)
            {
                if (this.Level.GameMode.Connected)
                {
                    this.ServerCommands.Add(Command.Id = Interlocked.Increment(ref this.NextServerCommandId), Command);
                    new Available_Server_Command(this.Level.GameMode.Device) {Command = Command}.Send();
                }
                else
                    Command.Execute();
            }
            else
                Logging.Info(this.GetType(), "AddCommand() - Command is not a server command.");
        }
    }
}
