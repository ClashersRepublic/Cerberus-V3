﻿namespace CR.Servers.CoC.Logic
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;

    internal class WorkerManagerV2
    {
        internal List<GameObject> GameObjects;
        internal int WorkerCount;

        public WorkerManagerV2()
        {
            this.GameObjects = new List<GameObject>();
            this.WorkerCount = 1;
        }

        internal int FreeWorkers
        {
            get
            {
                return this.WorkerCount - this.GameObjects.Count;
            }
        }

        internal void AllocateWorker(GameObject GameObject)
        {
            if (this.GameObjects.Contains(GameObject))
            {
                Logging.Error(this.GetType(), "AllocateWorker() called twice for same target!");
                return;
            }

            this.GameObjects.Add(GameObject);
        }

        internal void DeallocateWorker(GameObject GameObject)
        {
            if (!this.GameObjects.Remove(GameObject))
            {
                Logging.Error(this.GetType(), "DeallocateWorker() - GameObject is not in array!");
            }
        }

        internal GameObject GetShortestTaskGO()
        {
            if (this.GameObjects.Count > 0)
            {
                Task Task = new Task();

                this.GameObjects.ForEach(Construction =>
                {
                    int RemainingTime = -1;

                    if (Construction is Building Building)
                    {
                        HeroBaseComponent HeroBaseComponent = Building.HeroBaseComponent;
                        if (HeroBaseComponent != null)
                        {
                            if (HeroBaseComponent.Upgrading)
                            {
                                RemainingTime = HeroBaseComponent.RemainingUpgradeTime;
                            }
                        }
                        else if (!Building.Constructing)
                        {
                            Logging.Error(this.GetType(), "GetShortestTaskGO() : Worker allocated to building with remaining construction time 0");
                        }
                        else
                        {
                            RemainingTime = Building.RemainingConstructionTime;
                        }
                    }
                    else if (Construction is Obstacle Obstacle)
                    {
                        if (!Obstacle.ClearingOnGoing)
                        {
                            Logging.Error(this.GetType(), "GetShortestTaskGO() : Worker allocated to obstacle with remaining clearing time 0");
                        }
                        else
                        {
                            RemainingTime = Obstacle.RemainingClearingTime;
                        }
                    }
                    else if (Construction is Trap Trap)
                    {
                        if (!Trap.Constructing)
                        {
                            Logging.Error(this.GetType(), "GetShortestTaskGO() : Worker allocated to Trap with remaining construction time 0");
                        }
                        else
                        {
                            RemainingTime = Trap.RemainingConstructionTime;
                        }
                    }
                    else if (Construction is VillageObject VillageObject)
                    {
                        if (!VillageObject.Constructing)
                        {
                            Logging.Error(this.GetType(), "GetShortestTaskGO() : Worker allocated to village object with remaining clearing time 0");
                        }
                        else
                        {
                            RemainingTime = VillageObject.RemainingConstructionTime;
                        }
                    }

                    if (RemainingTime != -1)
                    {
                        if (Task.GameObject == null)
                        {
                            Task.GameObject = Construction;
                            Task.RemainingSeconds = RemainingTime;
                        }
                        else if (Task.RemainingSeconds > RemainingTime)
                        {
                            Task.GameObject = Construction;
                            Task.RemainingSeconds = RemainingTime;
                        }
                    }
                });

                return Task.GameObject;
            }

            return null;
        }

        internal void FinishTaskOfOneWorker()
        {
            GameObject GameObject = this.GetShortestTaskGO();
            if (GameObject != null)
            {
                if (GameObject is Building Building)
                {
                    HeroBaseComponent HeroBaseComponent = Building.HeroBaseComponent;
                    if (Building.Constructing)
                    {
                        Building.SpeedUpConstruction();
                    }
                    else if (HeroBaseComponent != null)
                    {
                        if (HeroBaseComponent.Upgrading)
                        {
                            HeroBaseComponent.SpeedUpUpgrade();
                        }
                    }
                }
                else if (GameObject is Obstacle Obstacle)
                {
                    if (Obstacle.ClearingOnGoing)
                    {
                        Obstacle.SpeedUpClearing();
                    }
                }
                else if (GameObject is Trap Trap)
                {
                    if (Trap.Constructing)
                    {
                        Trap.SpeedUpConstruction();
                    }
                }
                else if (GameObject is VillageObject VillageObject)
                {
                    if (VillageObject.Constructing)
                    {
                        VillageObject.SpeedUpConstruction();
                    }
                }
            }
        }

        private struct Task
        {
            internal GameObject GameObject;
            internal int RemainingSeconds;
        }
    }
}