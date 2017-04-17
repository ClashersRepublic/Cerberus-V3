using System.IO;
using Magic.Helpers;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class MissionProgressCommand : Command
    {
        public uint MissionID;
        public uint Tick;

        public MissionProgressCommand(PacketReader br)
        {
            MissionID = br.ReadUInt32();
            Tick = br.ReadUInt32();
        }
    }
}