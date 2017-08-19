using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Logic.Manager
{
    internal class WorkerManager_V2
    {
        internal List<Game_Object> GameObjectReferences;

        internal int WorkerCount;

        public WorkerManager_V2()
        {
            GameObjectReferences = new List<Game_Object>();
            WorkerCount = 1;
        }

        public static int GetFinishTaskOfOneWorkerCost() => 0;

        public static void RemoveGameObjectReferences(Game_Object go)
        {
        }

        public void AllocateWorker(Game_Object go)
        {
            if (GameObjectReferences.IndexOf(go) == -1)
            {
                 GameObjectReferences.Add(go);
            }
        }

        public void DeallocateWorker(Game_Object go)
        {
            if (GameObjectReferences.IndexOf(go) != -1)
            {
                GameObjectReferences.Remove(go);
            }
        }

        public void DecreaseWorkerCount() => WorkerCount--;

        public void FinishTaskOfOneWorker()
        {
            Game_Object go = GetShortestTaskGO;
            if (go != null)
            {
                if (go.ClassId == 10)
                {
                    var b = (Builder_Obstacle)go;
                    if (b.IsClearing)
                        b.SpeedUpClearing();
                }
                else
                {
                    var b = (Construction_Item)go;
                    if (b.IsConstructing)
                        b.SpeedUpConstruction();
                    else
                    {
                        /*var hero = b.GetHeroBaseComponent();
                        hero?.SpeedUpUpgrade();*/
                    }
                }
            }
        }

        public int GetFreeWorkers() => this.WorkerCount - this.GameObjectReferences.Count;

        public Game_Object GetShortestTaskGO
        {
            get
            {
                Game_Object shortestTaskGO = null;
                int shortestGOTime = 0;
                int currentGOTime;

                Parallel.ForEach((this.GameObjectReferences), go =>
                {
                    currentGOTime = -1;
                    if (go.ClassId == 10)
                    {
                        var c = (Builder_Obstacle)go;
                        if (c.IsClearing)
                        {
                            currentGOTime = c.GetRemainingClearingTime();
                        }
                    }
                    else
                    {
                        var c = (Construction_Item)go;
                        if (c.IsConstructing)
                        {
                            currentGOTime = c.GetRemainingConstructionTime;
                        }
                        else
                        {
                            /*var hero = c.GetHeroBaseComponent();
                            if (hero != null)
                            {
                                if (hero.IsUpgrading())
                                {
                                    currentGOTime = hero.GetRemainingUpgradeSeconds();
                                }
                            }*/
                        }
                    }

                    if (shortestTaskGO == null)
                    {
                        if (currentGOTime > -1)
                        {
                            shortestTaskGO = go;
                            shortestGOTime = currentGOTime;
                        }
                    }
                    else if (currentGOTime > -1)
                    {
                        if (currentGOTime < shortestGOTime)
                        {
                            shortestGOTime = currentGOTime;
                            shortestTaskGO = go;
                        }
                    }
                });
                return shortestTaskGO;
            }
        }

        public void IncreaseWorkerCount() => WorkerCount++;
    }
}
