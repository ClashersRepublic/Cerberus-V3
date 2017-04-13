﻿using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class CollectResourcesCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;

        public CollectResourcesCommand(PacketReader br)
        {
            BuildingId = br.ReadInt32(); 
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (gameObjectById == null || gameObjectById.ClassId != 0 && gameObjectById.ClassId != 4)
              return;
            ((ConstructionItem) gameObjectById).GetResourceProductionComponent(false).CollectResources();
        }
    }
}
