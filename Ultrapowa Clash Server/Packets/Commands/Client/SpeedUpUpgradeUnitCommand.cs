﻿using System.IO;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class SpeedUpUpgradeUnitCommand : Command
    {
        internal int m_vBuildingId;

        public SpeedUpUpgradeUnitCommand(PacketReader br)
        {
            m_vBuildingId = br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (gameObjectById == null || gameObjectById.ClassId != 0)
              return;
            UnitUpgradeComponent upgradeComponent = ((ConstructionItem)gameObjectById).GetUnitUpgradeComponent(false);
            if ((upgradeComponent != null ? upgradeComponent.GetCurrentlyUpgradedUnit() : (CombatItemData) null) == null)
                return;
            upgradeComponent.SpeedUp();
        }
    }
}
