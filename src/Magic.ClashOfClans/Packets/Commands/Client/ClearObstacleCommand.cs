using System;
using System.IO;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
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
