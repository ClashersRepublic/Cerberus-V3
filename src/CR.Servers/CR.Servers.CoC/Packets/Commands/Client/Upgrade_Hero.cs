﻿using System.Text;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Hero : Command
    {
        internal override int Type => 527;

        public Upgrade_Hero(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int BuildingId;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var level = this.Device.GameMode.Level;
            var gameObject = level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);
            if (gameObject != null)
            {
                if (gameObject is Building building)
                {
                    var HeroBaseComponent = building.HeroBaseComponent;
                    if (HeroBaseComponent != null)
                    {
                        if (HeroBaseComponent.UpgradeAvailable)
                        {
                            var HeroData = HeroBaseComponent.HeroData;
                            var heroLevel = level.Player.GetHeroUpgradeLevel(HeroData);

                            var resourceData = HeroData.UpgradeResourceData;
                            var upgradeCost = HeroData.UpgradeCost[heroLevel];

                            if (resourceData != null)
                            {
                                if (level.Player.Resources.GetCountByData(resourceData) >= upgradeCost)
                                {
                                    if (HeroData.VillageType == 0
                                        ? level.WorkerManager.FreeWorkers > 0
                                        : level.WorkerManagerV2.FreeWorkers > 0)
                                    {
                                        level.Player.Resources.Remove(resourceData, upgradeCost);
                                        HeroBaseComponent.StartUpgrade();
                                    }
                                    else
                                        Logging.Error(this.GetType(),
                                            "Unable to upgrade the hero. There is no free worker.");
                                }
                                else
                                    Logging.Error(this.GetType(),
                                        "Unable to upgrade the hero. The player doesn't have enough resources.");
                            }
                            else
                                Logging.Error(this.GetType(), "Unable to upgrade the hero. Resource data is null.");
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to upgrade the hero. Upgrade is not available.");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to upgrade the hero. The HeroBaseComponent is null.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to upgrade the hero. The game object is not a building.");
            }
            else
            {
                Logging.Error(this.GetType(), "Unable to upgrade the hero. The game object is null.");

                var Error = new StringBuilder();
                var Precise = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectByPreciseId(this.BuildingId);
                if (Precise != null)
                {
                    Error.AppendLine($"Building Id :  {this.BuildingId}");
                    Error.AppendLine($"Building Name :  {Precise.Data.Name}");
                    Error.AppendLine($"Building still null with precise id : {false}");

                    Error.AppendLine($"Player Id :  {this.Device.GameMode.Level.Player.UserId}");
                    Error.AppendLine($"Player current village :  {this.Device.GameMode.Level.GameObjectManager.Map}");
                    Error.AppendLine($"Player town hall level :  {this.Device.GameMode.Level.Player.TownHallLevel}");
                    Error.AppendLine($"Player town hall2 level :  {this.Device.GameMode.Level.Player.TownHallLevel2}");
                }
                else
                {
                    Error.AppendLine($"Precise building : null");
                    Error.AppendLine($"Building Id :  {this.BuildingId}");

                    Error.AppendLine($"Player Id :  {this.Device.GameMode.Level.Player.UserId}");
                    Error.AppendLine($"Player current village :  {this.Device.GameMode.Level.GameObjectManager.Map}");
                    Error.AppendLine($"Player town hall level :  {this.Device.GameMode.Level.Player.TownHallLevel}");
                    Error.AppendLine($"Player town hall2 level :  {this.Device.GameMode.Level.Player.TownHallLevel2}");
                }

                Resources.Logger.Debug(Error);
            }
        }
    }
}
