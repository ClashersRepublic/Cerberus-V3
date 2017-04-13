using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class MoveBuildingCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;
        public int X;
        public int Y;

        public MoveBuildingCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            BuildingId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            level.GameObjectManager.GetGameObjectByID(BuildingId).SetPositionXY(X, Y, level.GetPlayerAvatar().GetActiveLayout());
        }
    }
}