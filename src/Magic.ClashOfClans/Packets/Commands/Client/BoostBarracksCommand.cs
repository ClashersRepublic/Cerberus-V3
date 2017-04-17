using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;

namespace Magic.Packets.Commands.Client
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
