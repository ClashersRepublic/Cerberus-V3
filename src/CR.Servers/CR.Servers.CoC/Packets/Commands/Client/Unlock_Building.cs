using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Unlock_Building : Command
    {
        internal override int Type => 520;

        public Unlock_Building(Device device, Reader reader) : base(device, reader)
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
                if (gameObject.Type == 0)
                {
                    var building = gameObject as Building;

                    if (building != null)
                    {
                        if (building.Locked)
                        {
                            var data = building.BuildingData;

                            if (data.BuildCost[0] > 0)
                            {
                                var resourceData = data.BuildResourceData;
                                if (level.Player.Resources.GetCountByData(resourceData) >= data.BuildCost[0])
                                {
                                    level.Player.Resources.Remove(resourceData, data.BuildCost[0]);
                                    building.Locked = false;
                                }
                                else
                                    Logging.Error(this.GetType(), "Unable to unlock the building. The player doesn't have enough resources.");
                            }
                            else
                                building.Locked = false;
                            //Building.SetUpgradeLevel(Building.GetUpgradeLevel());
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to unlock the building. The building is already unlocked");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to unlock the building. The game object is not valid or not exist.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to unlock the building. The game object is not a building");
            }
            else
                Logging.Error(this.GetType(), "Unable to unlock the building. The game object is null");
        }
    }
}