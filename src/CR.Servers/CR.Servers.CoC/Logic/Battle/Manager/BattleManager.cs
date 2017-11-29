using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CR.Servers.CoC.Logic.Battle.Manager
{
    internal class BattleManager
    {
        internal Timer Timer;

        internal ConcurrentDictionary<long, Player> Spectators;

    }
}
