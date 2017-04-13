using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class ClearObstacleCommand : Command
    {
        public int ObstacleId;
        public uint Tick;

        public ClearObstacleCommand(PacketReader br)
        {
            ObstacleId = br.ReadInt32();
            Tick = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {         
        }
    }
}
