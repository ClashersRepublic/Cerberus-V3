using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Battle;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Search_Opponent_V2 : Command
    {
        internal override int Type => 601;

        public Search_Opponent_V2(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Execute()
        {
            new Pc_Battle_Data_V2(this.Device).Send();
        }
    }
}
