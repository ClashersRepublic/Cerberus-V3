using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class ToggleHeroAttackModeCommand : Command
    {
        public uint BuildingId { get; set; }

        public byte Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public ToggleHeroAttackModeCommand(PacketReader br)
        {
        }
    }
}