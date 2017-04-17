using System.IO;
using Magic.Helpers;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class ToggleAttackModeCommand : Command
    {
        public uint BuildingId { get; set; }

        public byte Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public ToggleAttackModeCommand(PacketReader br)
        {
        }
    }
}