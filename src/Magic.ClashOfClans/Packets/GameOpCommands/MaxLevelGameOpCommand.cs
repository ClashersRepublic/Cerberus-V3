﻿using Magic.Core.Network;
using Magic.Files.Logic;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.Packets.GameOpCommands
{
    internal class MaxLevelGameOpCommand : GameOpCommand
    {
        public MaxLevelGameOpCommand(string[] args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            var buildings = level.GameObjectManager.GetGameObjects(0);
            for (int i = 0; i < buildings.Count; i++)
            {
                var building = (Building)buildings[i];
                var data = (ConstructionItemData)building.Data;

                building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
            }

            var traps = level.GameObjectManager.GetGameObjects(4);
            for (int i = 0; i < traps.Count; i++)
            {
                var trap = (Trap)traps[i];
                var data = (ConstructionItemData)trap.Data;

                trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
            }

            new OwnHomeDataMessage(level.Client, level).Send();
        }
    }
}
