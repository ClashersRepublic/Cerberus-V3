﻿using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class EventsSeenCommand : Command
    {
        public int Tick;
        public int UnknownID;

        public EventsSeenCommand(PacketReader br)
        {
            Tick = br.ReadInt32();
            UnknownID = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}