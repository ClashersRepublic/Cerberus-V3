using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Core
{
    internal class Constants
    {
        internal const int ServerId = 0;
        internal const int SendBuffer = 2048 * 1;
        internal const int ReceiveBuffer = 2048 * 1;
        internal const int MaxPlayers = 1000 * 20;
        internal const int MaxSends = 1000 * 5;

        internal const DBMS Database = DBMS.Mongo;
    }
}
