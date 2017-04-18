﻿using System.IO;
using Magic.Core;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
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
            ClientAvatar avatar = level.Avatar;
            ConstructionItem gameObjectById = (ConstructionItem) level.GameObjectManager.GetGameObjectByID(BuildingId);
            BuildingData constructionItemData = (BuildingData) gameObjectById.GetConstructionItemData();

            if (!avatar.HasEnoughResources(constructionItemData.GetBuildResource(gameObjectById.GetUpgradeLevel()), constructionItemData.GetBuildCost(gameObjectById.GetUpgradeLevel())))
              return;
                string name = level.GameObjectManager.GetGameObjectByID(BuildingId).Data.Name;
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