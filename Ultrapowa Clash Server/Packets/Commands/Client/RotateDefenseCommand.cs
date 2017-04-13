using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class RotateDefenseCommand : Command
    {
        public int BuildingID { get; set; }

        public RotateDefenseCommand(PacketReader br)
        {
            BuildingID = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}
