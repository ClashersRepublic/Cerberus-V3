using System.Collections.Generic;
using System.Linq;
using CR.Servers.CoC.Packets;

namespace CR.Servers.CoC.Logic.Battle.Manager
{
    internal class BattleCommandManager
    {
        internal List<Command> Commands;
        internal List<Command> BufferedCommands;

        internal BattleManager BattleManager;

        public BattleCommandManager()
        {
            this.Commands = new List<Command>(512);
            this.BufferedCommands = new List<Command>();
        }

        public BattleCommandManager(BattleManager BattleManager) : this()
        {
            this.BattleManager = BattleManager;
        }

        internal void StoreCommands(Command Command)
        {
            this.BufferedCommands.Add(Command);
        }

        internal void Tick()
        {
            this.Commands.Clear();
            this.Commands.AddRange(this.BufferedCommands.Take(512));
            this.BufferedCommands.Clear();
        }
    }
}