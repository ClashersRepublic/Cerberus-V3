using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Packets;

namespace CR.Servers.CoC.Logic.Battle.Manager
{
    internal class BattleCommandManager
    {
        internal List<Command> Commands;
        internal List<Command> BufferedCommands;

        public BattleCommandManager()
        {
            this.Commands = new List<Command>(512);
            this.BufferedCommands = new List<Command>();
        }

        public BattleCommandManager(BattleManager BattleManager) : this()
        {
            //this.BattleManager = BattleManager;
        }

        internal void StoreCommands(List<Command> Commands)
        {
            this.BufferedCommands.AddRange(Commands);
        }

        internal void Tick()
        {
            this.Commands.AddRange(this.BufferedCommands);
            this.BufferedCommands.Clear();
        }
    }
}