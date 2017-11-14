using System.Collections.Generic;
using CR.Servers.CoC.Core;

namespace CR.Servers.CoC.Logic
{
    internal class WorkerManagerV2
    {
        internal int WorkerCount;

        internal List<GameObject> GameObjects;

        internal int FreeWorkers => this.WorkerCount - this.GameObjects.Count;

        public WorkerManagerV2()
        {
            this.GameObjects = new List<GameObject>();
            this.WorkerCount = 1;
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

                    switch (Construction.Type)
                    {
                        case 0:
                        {
                            Building Building = (Building)Construction;

                            if (!Building.Constructing)
                            {
                                Logging.Error(this.GetType(), "GetShortestTaskGO() : Worker allocated to building with remaining construction time 0");
                                break;
                            }

                            RemainingTime = Building.RemainingConstructionTime;

                            break;
                        }

                        case 3:
                        {
                            Obstacle Obstacle = (Obstacle)Construction;

                            if (!Obstacle.ClearingOnGoing)
                            {
                                Logging.Error(this.GetType(), "GetShortestTaskGO() : Worker allocated to obstacle with remaining clearing time 0");
                                break;
                            }

                            RemainingTime = Obstacle.RemainingClearingTime;

                            break;
                        }
                    }

                    if (RemainingTime != -1)
                    {
                        if (Task.RemainingSeconds > RemainingTime)
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

        private struct Task
        {
            internal GameObject GameObject;
            internal int RemainingSeconds;
        }
    }
}
