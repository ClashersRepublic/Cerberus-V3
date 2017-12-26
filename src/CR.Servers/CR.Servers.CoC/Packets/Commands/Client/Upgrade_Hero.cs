using CR.Servers.CoC.Core;
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
                                    if (HeroData.VillageType == 0 ? level.WorkerManager.FreeWorkers > 0 : level.WorkerManagerV2.FreeWorkers > 0)
                                    {
                                        level.Player.Resources.Remove(resourceData, upgradeCost);
                                        HeroBaseComponent.StartUpgrade();
                                    }
                                    else
                                        Logging.Error(this.GetType(), "Unable to upgrade the hero. There is no free worker.");
                                }
                                else
                                    Logging.Error(this.GetType(), "Unable to upgrade the hero. The player doesn't have enough resources.");
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
                Logging.Error(this.GetType(), "Unable to upgrade the hero. The game object is null.");
        }
    }
}
