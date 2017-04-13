using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
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