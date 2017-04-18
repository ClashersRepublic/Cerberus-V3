using System.Collections.Generic;
using System.IO;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class BoostBuildingCommand : Command
    {
        public int BoostedBuildingsCount { get; set; }

        public List<int> BuildingIds { get; set; }

        public BoostBuildingCommand(PacketReader br)
        {
            BuildingIds = new List<int>();

            BoostedBuildingsCount = br.ReadInt32();
            for (var i = 0; i < BoostedBuildingsCount; i++)
            {
                BuildingIds.Add(br.ReadInt32());
            }
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.Avatar;
            foreach (var buildingId in BuildingIds)
            {
                ConstructionItem gameObjectById = (ConstructionItem) level.GameObjectManager.GetGameObjectByID(buildingId);
                int diamondCount = ((BuildingData) gameObjectById.GetConstructionItemData()).BoostCost[gameObjectById.UpgradeLevel];
                if (avatar.HasEnoughDiamonds(diamondCount))
                {
                    gameObjectById.BoostBuilding();
                    avatar.SetDiamonds(avatar.GetDiamonds() - diamondCount);
                }
            }
        }
    }
}