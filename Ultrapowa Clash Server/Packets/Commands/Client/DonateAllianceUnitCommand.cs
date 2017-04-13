using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class DonateAllianceUnitCommand : Command
    {
        public uint PlayerId;
        public uint UnitType;
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        public DonateAllianceUnitCommand(PacketReader br)
        {
        }
    }
}
