using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
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