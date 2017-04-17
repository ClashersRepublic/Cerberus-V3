using System.IO;
using Magic.Helpers;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class CancelUpgradeUnitCommand : Command
    {
        public uint BuildingId { get; set; }

        public uint Unknown1 { get; set; }

        public CancelUpgradeUnitCommand(PacketReader br)
        {
        }
    }
}