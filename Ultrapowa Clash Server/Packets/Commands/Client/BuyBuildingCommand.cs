using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class BuyBuildingCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;
        public int X;
        public int Y;

        public BuyBuildingCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            BuildingId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();
            BuildingData dataById = (BuildingData) CSVManager.DataTables.GetDataById(BuildingId);
            Building building = new Building((Data) dataById, level);

            if (!avatar.HasEnoughResources(dataById.GetBuildResource(0), dataById.GetBuildCost(0)) || !dataById.IsWorkerBuilding() && !level.HasFreeWorkers())
                return;
            ResourceData buildResource = dataById.GetBuildResource(0);
            avatar.CommodityCountChangeHelper(0, (Data)buildResource, -dataById.GetBuildCost(0));
            building.StartConstructing(X, Y);
            level.GameObjectManager.AddGameObject((GameObject)building);
        }
    }
}
