using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Buy_Building : Command
    {
        internal override int Type => 500;

        public Buy_Building(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int X;
        internal int Y;

        internal BuildingData BuildingData;


        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();

            this.BuildingData = this.Reader.ReadData<BuildingData>();

            base.Decode();
        }

        internal override void Execute()
        {
            if (this.BuildingData != null)
            {
                var Level = Device.GameMode.Level;
                //if (!Level.IsBuildingCapReached(this.BuildingData))
                {
                    BuildingClassData BuildingClassData = (BuildingClassData)CSV.Tables.Get(Gamefile.Building_Classes).GetData(this.BuildingData.BuildingClass);
                    ResourceData ResourceData = this.BuildingData.BuildResourceData;

                    if (BuildingClassData.CanBuy)
                    {
                        if (this.BuildingData.VillageType == 0)
                        {
                            if (this.BuildingData.TownHallLevel[0] <= Level.GameObjectManager.TownHall.GetUpgradeLevel() + 1)
                            {
                                if (this.BuildingData.IsWorker)
                                {
                                    int Cost;

                                    switch (Level.WorkerManager.WorkerCount)
                                    {
                                        case 1:
                                            Cost = Globals.WorkerCost2Nd;
                                            break;
                                        case 2:
                                            Cost = Globals.WorkerCost3Rd;
                                            break;
                                        case 3:
                                            Cost = Globals.WorkerCost4Th;
                                            break;
                                        case 4:
                                            Cost = Globals.WorkerCost5Th;
                                            break;

                                        default:
                                            Cost = this.BuildingData.BuildCost[0];
                                            break;
                                    }

                                    if (Level.Player.HasEnoughDiamonds(Cost))
                                    {
                                        Level.Player.UseDiamonds(Cost);
                                        this.StartConstruction(Level);
                                    }
                                    return;
                                }

                                if (Level.Player.Resources.GetCountByData(ResourceData) >= this.BuildingData.BuildCost[0])
                                {
                                    if (Level.WorkerManager.FreeWorkers > 0)
                                    {
                                        Level.Player.Resources.Remove(ResourceData, this.BuildingData.BuildCost[0]);
                                        this.StartConstruction(Level);
                                    }
                                    //else
                                        //Logging.Error(this.GetType(), "Unable to buy building. The player doesn't have any free worker!");
                                }
                            }
                        }
                        else
                        {
                            if (this.BuildingData.TownHallLevel2[0] <=  Level.GameObjectManager.TownHall2.GetUpgradeLevel() + 1)
                            {
                                if (this.BuildingData.IsTroopHousingV2)
                                {
                                    var TroopHousing = Level.GameObjectManager.Filter.GetGameObjectCount(this.BuildingData);

                                    int Cost = Globals.TroopHousingV2Cost[TroopHousing];
                                    int Time = Globals.TroopHousingV2BuildTimeSeconds[TroopHousing];
                                    if (Level.Player.Resources.GetCountByData(ResourceData) >= Cost)
                                    {
                                        if (Level.WorkerManagerV2.FreeWorkers > 0)
                                        {
                                            Level.Player.Resources.Remove(ResourceData, Cost);
                                            this.StartConstruction(Level, Time);
                                        }
                                        //else
                                           // Logging.Error(this.GetType(), "Unable to buy building. The player doesn't have any free worker!");
                                    }
                                    return;
                                }

                                if (this.BuildingData.IsWorker2)
                                {
                                    int Cost;

                                    switch (Level.WorkerManagerV2.WorkerCount)
                                    {
                                        case 1:
                                            Cost = Globals.WorkerCost2Nd;
                                            break;
                                        case 2:
                                            Cost = Globals.WorkerCost3Rd;
                                            break;
                                        case 3:
                                            Cost = Globals.WorkerCost4Th;
                                            break;
                                        case 4:
                                            Cost = Globals.WorkerCost5Th;
                                            break;

                                        default:
                                            Cost = this.BuildingData.BuildCost[0];
                                            break;

                                    }

                                    if (Level.Player.HasEnoughDiamonds(Cost))
                                    {
                                        Level.Player.UseDiamonds(Cost);
                                        this.StartConstruction(Level);
                                    }
                                    return;
                                }

                                if (Level.Player.Resources.GetCountByData(ResourceData) >=  this.BuildingData.BuildCost[0])
                                {
                                    if (Level.WorkerManagerV2.FreeWorkers > 0)
                                    {
                                        Level.Player.Resources.Remove(ResourceData, this.BuildingData.BuildCost[0]);
                                        this.StartConstruction(Level);
                                    }
                                    //else
                                        //Logging.Error(this.GetType(), "Unable to buy building. The player doesn't have any free worker!");
                                }
                            }
                        }
                    }
                }
            }
            else
                Logging.Error(this.GetType(), $"Unable to buy building. The building data is null! ");
        }

        internal void StartConstruction(Level Level)
        {
            Building GameObject = new Building(this.BuildingData, Level);

            GameObject.SetUpgradeLevel(-1);

            GameObject.Position.X = this.X << 9;
            GameObject.Position.Y = this.Y << 9;

            if (this.BuildingData.VillageType == 0)
                Level.WorkerManager.AllocateWorker(GameObject);
            else
                Level.WorkerManagerV2.AllocateWorker(GameObject);

            if (this.BuildingData.GetBuildTime(0) <= 0)
            {
                GameObject.FinishConstruction();
            }
            else
            {
                GameObject.ConstructionTimer = new Timer();
                GameObject.ConstructionTimer.StartTimer(Level.Player.LastTick, this.BuildingData.GetBuildTime(0));
            }

            Level.GameObjectManager.AddGameObject(GameObject, Level.Player.Map);
        }

        internal void StartConstruction(Level Level, int Time)
        {
            Building GameObject = new Building(this.BuildingData, Level);

            GameObject.SetUpgradeLevel(-1);

            GameObject.Position.X = this.X << 9;
            GameObject.Position.Y = this.Y << 9;

            if (this.BuildingData.VillageType == 0)
                Level.WorkerManager.AllocateWorker(GameObject);
            else
                Level.WorkerManagerV2.AllocateWorker(GameObject);

            if (Time <= 0)
            {
                GameObject.FinishConstruction();
            }
            else
            {
                GameObject.ConstructionTimer = new Timer();
                GameObject.ConstructionTimer.StartTimer(Level.Player.LastTick, Time);
            }

            Level.GameObjectManager.AddGameObject(GameObject, Level.Player.Map);
        }
    }
}
