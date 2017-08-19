using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic.Manager;
using Magic.ClashOfClans.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Logic
{
    internal class Level
    {
        public GameObjectManager GameObjectManager;
        public WorkerManager VillageWorkerManager;
        public WorkerManager_V2 BuilderWorkerManager;

        public Level()
        {
            VillageWorkerManager = new WorkerManager();
            BuilderWorkerManager = new WorkerManager_V2();
            GameObjectManager = new GameObjectManager(this);

            Avatar = new Avatar();
        }

        public Level(long id)
        {
            VillageWorkerManager = new WorkerManager();
            BuilderWorkerManager = new WorkerManager_V2();
            GameObjectManager = new GameObjectManager(this);

            Avatar = new Avatar(id);
        }

        public Avatar Avatar { get; set; }
        public Device Device { get; set; }

        public ComponentManager GetComponentManager() => GameObjectManager.GetComponentManager();

        public bool HasFreeVillageWorkers => VillageWorkerManager.GetFreeWorkers() > 0;
        public bool HasFreeBuilderWorkers => BuilderWorkerManager.GetFreeWorkers() > 0;


        public string Json
        {
            get => JsonConvert.SerializeObject(GameObjectManager.Save());
            set => GameObjectManager.Load(JObject.Parse(value));
        }

        public void SetHome(string jsonHome)
        {
            var gameObjects = GameObjectManager.GetAllGameObjects();
            foreach (List<Game_Object> t in gameObjects)
                t.Clear();

            GameObjectManager.Load(JObject.Parse(jsonHome));
        }

        public void Tick()
        {
            Avatar.LastTick = DateTime.UtcNow;
            GameObjectManager.Tick();
        }
    }
}
