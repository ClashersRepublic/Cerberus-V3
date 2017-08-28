using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    //This Command Only
    internal class Trap_Triggered : Command
    {
        public Trap_Triggered(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
        }
    }
}
