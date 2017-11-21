using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class SpeedUp_Upgrade_Unit : Command
    {
        internal override int Type => 504;

        public SpeedUp_Upgrade_Unit(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int BuildingId;

        internal override void Decode()
        {
            this.BuildingId = Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var gameObject =  this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);

            if (gameObject != null)
            {
                if (gameObject.Type == 0)
                {
                    var building = gameObject as Building;
                    if (building != null)
                    {
                        var unitUpgradeComponent = building.UnitUpgradeComponent;

                        if (unitUpgradeComponent != null)
                        {
                            unitUpgradeComponent.SpeedUp();
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to speed up the upgrade. The game object doesn't contain a UnitUpgradeComponent.");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to speed up the upgrade. The game object is not valid or not exist.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to speed up the upgrade. The game object is not a building");
            }
            else
                Logging.Error(this.GetType(), "Unable to speed up the upgrade. The game object is null");
        }
    }
}
