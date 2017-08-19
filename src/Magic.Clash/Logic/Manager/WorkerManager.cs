using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Logic.Manager
{
    internal class WorkerManager
    {
        readonly List<Game_Object> _gameObjectReferences;
       
        int _workerCount;

        public WorkerManager()
        {
            _gameObjectReferences = new List<Game_Object>();
            _workerCount = 0;
        }

        public static int GetFinishTaskOfOneWorkerCost() => 0;

        public static void RemoveGameObjectReferences(Game_Object go)
        {
        }

        public void AllocateWorker(Game_Object go)
        {
            if (_gameObjectReferences.IndexOf(go) == -1)
            {
                _gameObjectReferences.Add(go);
            }
        }

        public void DeallocateWorker(Game_Object go)
        {
            if (_gameObjectReferences.IndexOf(go) != -1)
            {
                _gameObjectReferences.Remove(go);
            }
        }

        public void DecreaseWorkerCount() => _workerCount--;

        public void FinishTaskOfOneWorker()
        {
            var go = GetShortestTaskGo();
            if (go != null)
            {
                if (go.ClassId == 3)
                {
                    /*var b = (Obstacle)go;
                    if (b.IsClearing)
                        b.SpeedUpClearing();*/
                }
                else
                {
                    /*var b = (ConstructionItem)go;
                    if (b.IsConstructing())
                        b.SpeedUpConstruction();
                    else
                    {
                        var hero = b.GetHeroBaseComponent();
                        if (hero != null)
                            hero.SpeedUpUpgrade();
                    }*/
                }
            }
        }

        public int GetFreeWorkers() => _workerCount - _gameObjectReferences.Count;

        public Game_Object GetShortestTaskGo()
        {
            Game_Object shortestTaskGO = null;
            int shortestGOTime = 0;
            int currentGOTime;

			Parallel.ForEach((_gameObjectReferences), go =>
			{
				currentGOTime = -1;
				if (go.ClassId == 3)
				{
				    /*var c = (Obstacle)go;
				    if (c.IsClearing)
				    {
				        currentGOTime = c.GetRemainingClearingTime();
				    }*/
                }
				else
				{
					/*if (c.IsConstructing())
					{
						currentGOTime = c.GetRemainingConstructionTime();
					}
					else
					{
						var hero = c.GetHeroBaseComponent();
						if (hero != null)
						{
							if (hero.IsUpgrading())
							{
								currentGOTime = hero.GetRemainingUpgradeSeconds();
							}
						}
					}*/
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

        public int GetTotalWorkers() => _workerCount;

        public void IncreaseWorkerCount() => _workerCount++;
    }
}
