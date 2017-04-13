﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.Packets.Commands.Client
{
    internal class StartClanWarCommand : Command
    {
        public int Tick;

        public StartClanWarCommand(PacketReader br)
        {
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}
