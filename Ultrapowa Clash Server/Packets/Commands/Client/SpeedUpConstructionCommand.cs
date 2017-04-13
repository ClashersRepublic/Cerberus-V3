﻿using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class SpeedUpConstructionCommand : Command
    {
        internal int m_vBuildingId;

        public SpeedUpConstructionCommand(PacketReader br)
        {
            m_vBuildingId = br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (gameObjectById == null || gameObjectById.ClassId != 0 && gameObjectById.ClassId != 4)
              return;
            ((ConstructionItem) gameObjectById).SpeedUpConstruction();
        }
    }
}