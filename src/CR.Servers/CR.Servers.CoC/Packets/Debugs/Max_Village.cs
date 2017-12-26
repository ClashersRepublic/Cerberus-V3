using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Debugs
{
    internal class Max_Village : Debug
    {
        internal override Rank RequiredRank => Rank.Elite;

        internal int VillageID;
        public Max_Village(Device Device, params string[] Parameters) : base(Device, Parameters)
        {

        }

        internal StringBuilder Help;
        internal override void Process()
        {
           
        }
    }
}
