using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Network.Debugs;

namespace Magic.ClashOfClans.Network
{
    internal static class DebugFactory
    {
        internal const string Delimiter = "/";

        public static Dictionary<string, Type> Debugs;

        static DebugFactory()
        {
            Debugs = new Dictionary<string, Type>
            {
                { "max_village", typeof(Max_Village)},
            };
        }
    }
}