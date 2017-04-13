using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.Packets.Commands.Client
{
    internal class BoostBarracksCommand : Command
    {
        public long Tick { get; set; }

        public BoostBarracksCommand(PacketReader br) 
        {
            Tick = br.ReadInt64();
        }

        public override void Execute(Level level)
        {
        }
    }
}
