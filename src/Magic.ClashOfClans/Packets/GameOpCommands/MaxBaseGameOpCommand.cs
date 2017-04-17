using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Files.Logic;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.Packets.GameOpCommands
{
    internal class MaxBaseGameOpCommand : GameOpCommand
    {
        public static readonly string s_maxBase = File.ReadAllText("contents/max_home.json");

        public MaxBaseGameOpCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            //var buildings = level.GameObjectManager.GetGameObjects(0);
            //for (int i = 0; i < buildings.Count; i++)
            //{
            //    var building = (Building)buildings[i];
            //    var data = (ConstructionItemData)building.GetData();

            //    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
            //}

            //var traps = level.GameObjectManager.GetGameObjects(4);
            //for (int i = 0; i < traps.Count; i++)
            //{
            //    var trap = (Trap)traps[i];
            //    var data = (ConstructionItemData)trap.GetData();

            //    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
            //}

            level.SetHome(s_maxBase);
            new OwnHomeDataMessage(level.GetClient(), level).Send();
        }
    }
}
