﻿using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class ClientServerTickCommand : Command
    {
        public int Tick;
        public int Unknown1;

        public ClientServerTickCommand(PacketReader br)
        {
            Unknown1 = br.ReadInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}