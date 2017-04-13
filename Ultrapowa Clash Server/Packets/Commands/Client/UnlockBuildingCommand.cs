using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class UnlockBuildingCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;

        public UnlockBuildingCommand(PacketReader br)
        {
            BuildingId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();
            ConstructionItem gameObjectById = (ConstructionItem) level.GameObjectManager.GetGameObjectByID(BuildingId);
            BuildingData constructionItemData = (BuildingData) gameObjectById.GetConstructionItemData();

            if (!avatar.HasEnoughResources(constructionItemData.GetBuildResource(gameObjectById.GetUpgradeLevel()), constructionItemData.GetBuildCost(gameObjectById.GetUpgradeLevel())))
              return;
                string name = level.GameObjectManager.GetGameObjectByID(BuildingId).GetData().GetName();
                Logger.Write("Unlocking Building: " + name + " (" + BuildingId + ')');
                if (string.Equals(name, "Alliance Castle"))
                {
                  avatar.IncrementAllianceCastleLevel();
                  BuildingData buildingData = ((Building) level.GameObjectManager.GetGameObjectByID(BuildingId)).GetBuildingData();
                  avatar.SetAllianceCastleTotalCapacity(buildingData.GetUnitStorageCapacity(avatar.GetAllianceCastleLevel()));
                }
                ResourceData buildResource = constructionItemData.GetBuildResource(gameObjectById.GetUpgradeLevel());
                avatar.SetResourceCount(buildResource, avatar.GetResourceCount(buildResource) - constructionItemData.GetBuildCost(gameObjectById.GetUpgradeLevel()));
                gameObjectById.Unlock();
            }
        }
    }