using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Packets;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic.Battle.Slots
{
    internal class Battle_Command : List<Command>
    {
        internal JArray Save()
        {
            JArray Array = new JArray();

            this.ForEach(Item =>
            {
                Array.Add(Item.Save());
            });

            return Array;
        }
    }
}
