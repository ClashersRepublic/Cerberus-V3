using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Unit : Command
    {
        internal override int Type => 516;

        public Upgrade_Unit(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal Data Unit;
        internal int UnitType;
        internal int BuildingId;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.UnitType = this.Reader.ReadInt32();
            this.Unit = this.Reader.ReadData();
            base.Decode();
        }

        internal override void Execute()
        {
            var level = this.Device.GameMode.Level;
            if (this.Unit != null)
            {
                var gameObject = level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);
                if (gameObject != null)
                {
                    if (gameObject is Building building)
                    {
                        var unitUpgradeComponent = building.UnitUpgradeComponent;

                        if (unitUpgradeComponent != null)
                        {
                            var unitLevel = level.Player.GetUnitUpgradeLevel(this.Unit);

                            var resourceData = this.UnitType == 1 ? ((SpellData)Unit).UpgradeResourceData  : ((CharacterData)Unit).UpgradeResourceData;
                            var upgradeCost = this.UnitType == 1 ? ((SpellData)Unit).UpgradeCost[unitLevel] : ((CharacterData)Unit).UpgradeCost[unitLevel];

                            if (resourceData != null)
                            {
                                if (level.Player.Resources.GetCountByData(resourceData) >= upgradeCost)
                                {
                                    if (unitUpgradeComponent.CanStartUpgrading(this.Unit))
                                    {
                                        level.Player.Resources.Remove(resourceData, upgradeCost);
                                        unitUpgradeComponent.StartUpgrading(this.Unit);
                                    }
                                    else
                                        Logging.Error(this.GetType(),
                                            "Unable to upgrade the unit. The UnitUpgradeComponent probably training other unit.");
                                }
                                else
                                    Logging.Error(this.GetType(),
                                        "Unable to upgrade the unit. The player doesn't have enough resources.");
                            }
                            else
                                Logging.Error(this.GetType(),
                                    "Unable to upgrade the unit. The resources data is null.");
                        }
                        else
                            Logging.Error(this.GetType(),
                                "Unable to upgrade the unit. The game object doesn't contain a UnitUpgradeComponent.");
                    }
                    else
                        Logging.Error(this.GetType(),
                            "Unable to upgrade the unit. The game object is not valid or not exist");
                }
                else
                    Logging.Error(this.GetType(), "Unable to upgrade the unit. The game object is null");
            }
            else
                Logging.Error(this.GetType(), "Unable to to upgrade the unit. The unit data is null");
        }
    }
}