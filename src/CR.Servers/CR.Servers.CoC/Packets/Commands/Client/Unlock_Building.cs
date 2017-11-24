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
                if (gameObject is Building building)
                {
                    if (building.Locked)
                    {
                        var data = building.BuildingData;
                        var resourceData = data.BuildResourceData;
                        if (level.Player.Resources.GetCountByData(resourceData) >= data.BuildCost[0])
                        {
                            if (data.VillageType == 0)
                            {
                                level.Player.Resources.Remove(resourceData, data.BuildCost[0]);
                                if (data.IsAllianceCastle)
                                {
                                    level.Player.CastleLevel++;
                                    level.Player.CastleTotalCapacity = data.HousingSpace[level.Player.CastleLevel];
                                    level.Player.CastleTotalSpellCapacity = data.HousingSpaceAlt[level.Player.CastleLevel];
                                }
                                building.Locked = false;

                            }
                            else
                            {

                                if (level.WorkerManagerV2.FreeWorkers > 0)
                                {
                                    level.Player.Resources.Remove(resourceData, data.BuildCost[0]);
                                    building.SetUpgradeLevel(-1);

                                    level.WorkerManagerV2.AllocateWorker(building);

                                    if (data.GetBuildTime(0) <= 0)
                                    {
                                        building.FinishConstruction();
                                    }
                                    else
                                    {
                                        building.ConstructionTimer = new Timer();
                                        building.ConstructionTimer.StartTimer(level.Player.LastTick,
                                            data.GetBuildTime(0));
                                    }
                                }
                                else
                                    Logging.Error(this.GetType(),
                                        "Unable to unlock the building. The player doesn't have any free worker.");
                            }
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to unlock the building. The player doesn't have enough resources.");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to unlock the building. The building is already unlocked");
                }
                else
                    Logging.Error(this.GetType(), "Unable to unlock the building. The game object is not a building.");
            }
            else
                Logging.Error(this.GetType(), "Unable to unlock the building. The game object is null");
        }
    }
}