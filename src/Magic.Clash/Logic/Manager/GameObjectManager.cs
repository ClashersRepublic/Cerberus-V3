using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;

namespace Magic.ClashOfClans.Logic.Manager
{
    internal class GameObjectManager
    {
        public GameObjectManager(Level level)
        {
            Level = level;
            GameObjects = new List<List<Game_Object>>();
            GameObjectRemoveList = new List<Game_Object>();
            GameObjectsIndex = new List<int>();
            for (var i = 0; i < 16; i++)
            {
                GameObjects.Add(new List<Game_Object>());
                GameObjectsIndex.Add(0);
            }

            ComponentManager = new ComponentManager(Level);
        }

        private readonly ComponentManager ComponentManager;
        private readonly List<Game_Object> GameObjectRemoveList;
        private readonly List<List<Game_Object>> GameObjects;
        private readonly List<int> GameObjectsIndex;
        private readonly Level Level;

        public void AddGameObject(Game_Object go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorkerBuilding())
                    Level.VillageWorkerManager.IncreaseWorkerCount();
            }
            else if (go.ClassId == 7)
            {
                var b = (Builder_Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorker2Building())
                    Level.BuilderWorkerManager.IncreaseWorkerCount();
            }
            GameObjects[go.ClassId].Add(go);
        }

        public List<List<Game_Object>> GetAllGameObjects()
        {
            return GameObjects;
        }

        public ComponentManager GetComponentManager()
        {
            return ComponentManager;
        }


        public Game_Object GetGameObjectByID(int globalId, bool builder)
        {
            var classId = builder ? GlobalId.GetType(globalId) - 493 : GlobalId.GetType(globalId) - 500;
            if (GameObjects.Count < classId)
                return null;

            try
            {
                return GameObjects[classId].Find(g => g.GlobalId == globalId);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ExceptionLogger.Log(e, $"GameObjects throw ArgumentOutOfRangeException for {classId} with Global Id {globalId} ");
                return null;
            }
        }

        public List<Game_Object> GetGameObjects(int classId)
        {
            return GameObjects[classId];
        }

        public void Load(JObject jsonObject)
        {
            var jsonBuildings = (JArray) jsonObject["buildings"];
            foreach (JObject jsonBuilding in jsonBuildings)
            {
                var bd =
                    CSV.Tables.Get(Gamefile.Buildings)
                        .GetDataWithID(jsonBuilding["data"].ToObject<int>()) as Buildings;
                var b = new Building(bd, Level);
                AddGameObject(b);
                b.Load(jsonBuilding);
            }

            var jsonObstacles = (JArray) jsonObject["obstacles"];
            foreach (JObject jsonObstacle in jsonObstacles)
            {
                var dd =
                    CSV.Tables.Get(Gamefile.Obstacles).GetDataWithID(jsonObstacle["data"].ToObject<int>()) as Obstacles;
                var d = new Obstacle(dd, Level);
                AddGameObject(d);
                d.Load(jsonObstacle);
            }

            var jsonTraps = (JArray) jsonObject["traps"];
            foreach (JObject jsonTrap in jsonTraps)
            {
                var td = CSV.Tables.Get(Gamefile.Traps).GetDataWithID(jsonTrap["data"].ToObject<int>()) as Traps;
                var t = new Trap(td, Level);
                AddGameObject(t);
                t.Load(jsonTrap);
            }

            var jsonDecos = (JArray) jsonObject["decos"];
            foreach (JObject jsonDeco in jsonDecos)
            {
                var dd = CSV.Tables.GetWithGlobalID(jsonDeco["data"].ToObject<int>()) as Decos;
                var d = new Deco(dd, Level);
                AddGameObject(d);
                d.Load(jsonDeco);
            }

            var villageObjects = (JArray) jsonObject["vobjs"];
            foreach (JObject villageObject in villageObjects)
            {
                var dd = CSV.Tables.GetWithGlobalID(villageObject["data"].ToObject<int>()) as Village_Objects;
                var d = new Village_Object(dd, Level);
                AddGameObject(d);
                d.Load(villageObject);
            }

            var jsonBuildings2 = (JArray) jsonObject["buildings2"];
            foreach (JObject jsonBuilding2 in jsonBuildings2)
            {
                var bd =
                    CSV.Tables.Get(Gamefile.Buildings)
                        .GetDataWithID(jsonBuilding2["data"].ToObject<int>()) as Buildings;
                var b = new Builder_Building(bd, Level);
                AddGameObject(b);
                b.Load(jsonBuilding2);
            }

            var jsonObstacles2 = (JArray) jsonObject["obstacles2"];
            foreach (JObject jsonObstacle2 in jsonObstacles2)
            {
                var dd =
                    CSV.Tables.Get(Gamefile.Obstacles)
                        .GetDataWithID(jsonObstacle2["data"].ToObject<int>()) as Obstacles;
                var d = new Builder_Obstacle(dd, Level);
                AddGameObject(d);
                d.Load(jsonObstacle2);
            }

            var jsonTraps2 = (JArray) jsonObject["traps2"];
            foreach (JObject jsonTrap2 in jsonTraps2)
            {
                var td = CSV.Tables.Get(Gamefile.Traps).GetDataWithID(jsonTrap2["data"].ToObject<int>()) as Traps;
                var t = new Builder_Trap(td, Level);
                AddGameObject(t);
                t.Load(jsonTrap2);
            }

            var jsonDecos2 = (JArray) jsonObject["decos2"];
            foreach (JObject jsonDeco2 in jsonDecos2)
            {
                var dd = CSV.Tables.GetWithGlobalID(jsonDeco2["data"].ToObject<int>()) as Decos;
                var d = new Builder_Deco(dd, Level);
                AddGameObject(d);
                d.Load(jsonDeco2);
            }

            var jsonObjects2 = (JArray) jsonObject["vobjs2"];
            foreach (JObject jsonObject2 in jsonObjects2)
            {
                var dd = CSV.Tables.GetWithGlobalID(jsonObject2["data"].ToObject<int>()) as Village_Objects;
                var d = new Builder_Village_Object(dd, Level);
                AddGameObject(d);
                d.Load(jsonObject2);
            }
        }

        public void RemoveGameObject(Game_Object go)
        {
            GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorkerBuilding())
                    Level.VillageWorkerManager.DecreaseWorkerCount();
            }
            else if (go.ClassId == 7)
            {
                var b = (Builder_Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorker2Building())
                    Level.BuilderWorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(Game_Object go)
        {
            ComponentManager.RemoveGameObjectReferences(go);
        }

        public JObject Save()
        {
            var JBuildings = new JArray();
            var c = 0;
            foreach (var go in new List<Game_Object>(GameObjects[0]))
            {
                var b = (Building) go;
                var j = new JObject {{"data", b.GetBuildingData.GetGlobalId()}, {"id", 500000000 + c}};
                b.Save(j);
                JBuildings.Add(j);
                c++;
            }

            var JObstacles = new JArray();
            var o = 0;
            foreach (var go in new List<Game_Object>(GameObjects[3]))
            {
                var d = (Obstacle) go;
                var j = new JObject {{"data", d.GetObstacleData().GetGlobalId()}, {"id", 503000000 + o}};
                d.Save(j);
                JObstacles.Add(j);
                o++;
            }

            var JTraps = new JArray();
            var u = 0;
            foreach (var go in new List<Game_Object>(GameObjects[4]))
            {
                var t = (Trap) go;
                var j = new JObject {{"data", t.GetTrapData.GetGlobalId()}, {"id", 504000000 + u}};
                t.Save(j);
                JTraps.Add(j);
                u++;
            }

            var JDecos = new JArray();
            var e = 0;
            foreach (var go in new List<Game_Object>(GameObjects[6]))
            {
                var d = (Deco) go;
                var j = new JObject {{"data", d.GetDecoData().GetGlobalId()}, {"id", 506000000 + e}};
                d.Save(j);
                JDecos.Add(j);
                e++;
            }


            var JObject = new JArray();
            var jO = 0;
            foreach (var go in new List<Game_Object>(GameObjects[8]))
            {
                var d = (Village_Object) go;
                var j = new JObject {{"data", d.GetVillageObjectsData.GetGlobalId()}, {"id", 508000000 + jO}};
                d.Save(j);
                JObject.Add(j);
                jO++;
            }

            var JBuildings2 = new JArray();
            var c2 = 0;
            foreach (var go in new List<Game_Object>(GameObjects[7]))
            {
                var b = (Builder_Building) go;
                var j = new JObject {{"data", b.GetBuildingData.GetGlobalId()}, {"id", 500000000 + c2}};
                b.Save(j);
                JBuildings2.Add(j);
                c2++;
            }

            var JObstacles2 = new JArray();
            var o2 = 0;
            foreach (var go in new List<Game_Object>(GameObjects[10]))
            {
                var d = (Builder_Obstacle) go;
                var j = new JObject {{"data", d.GetObstacleData.GetGlobalId()}, {"id", 503000000 + o2}};
                d.Save(j);
                JObstacles2.Add(j);
                o2++;
            }

            var JTraps2 = new JArray();
            var u2 = 0;
            foreach (var go in new List<Game_Object>(GameObjects[11]))
            {
                var t = (Builder_Trap) go;
                var j = new JObject {{"data", t.GetTrapData.GetGlobalId()}, {"id", 504000000 + u2}};
                t.Save(j);
                JTraps2.Add(j);
                u2++;
            }


            var JDecos2 = new JArray();
            var e2 = 0;
            foreach (var go in new List<Game_Object>(GameObjects[13]))
            {
                var d = (Builder_Deco) go;
                var j = new JObject {{"data", d.GetDecoData.GetGlobalId()}, {"id", 506000000 + e2}};
                d.Save(j);
                JDecos2.Add(j);
                e2++;
            }

            var JObject2 = new JArray();
            var jO2 = 0;
            foreach (var go in new List<Game_Object>(GameObjects[15]))
            {
                var d = (Builder_Village_Object) go;
                var j = new JObject {{"data", d.GetVillageObjectsData.GetGlobalId()}, {"id", 508000000 + jO2}};
                d.Save(j);
                JObject2.Add(j);
                jO2++;
            }

            var jsonData = new JObject
            {
                {"exp_ver", 1},
                {"android_client", true},
                {"active_layout", 0},
                {"act_l2", 0},
                {"war_layout", 0},
                {"layout_state", new JArray {0, 0, 0, 0, 0, 0, 0, 0}},
                {"layout_state2", new JArray {0, 0, 0, 0, 0, 0, 0, 0}},
                {"layout_cooldown", new JArray {0, 0, 0, 0, 0, 0, 0, 0}},
                {"buildings", JBuildings},
                {"obstacles", JObstacles},
                {"traps", JTraps},
                {"decos", JDecos},
                {"vobjs", JObject},
                {"units", new JArray {"unit_prod"}},
                {"spells", new JArray {"unit_prod"}},
                {"buildings2", JBuildings2},
                {"obstacles2", JObstacles2},
                {"traps2", JTraps2},
                {"decos2", JDecos2},
                {"vobjs2", JObject2},
                {"offer", new JObject()},
                {"troop_req_msg", "Sup"},
                {"last_league_rank", 12},
                {"last_alliance_level", 1},
                {"last_league_shuffle", 0},
                {"last_season_seen", 12},
                {"last_news_seen", 999999999},
                {"war_tutorials_seen", 0},
                {"war_base", true},
                {"arr_war_base", false},
                {"account_flags", 15},
                {"bool_layout_edit_shown_erase", true}
            };

            return jsonData;
        }

        public void Tick()
        {
            ComponentManager.Tick();
            foreach (var l in GameObjects)
            foreach (var go in l)
                go.Tick();
            foreach (var g in new List<Game_Object>(GameObjectRemoveList))
            {
                RemoveGameObjectTotally(g);
                GameObjectRemoveList.Remove(g);
            }
        }

        private int GenerateGameObjectGlobalId(Game_Object go)
        {
            var index = GameObjectsIndex[go.ClassId];
            GameObjectsIndex[go.ClassId]++;
            return go.Builder
                ? GlobalId.CreateGlobalId(go.ClassId + 493, index)
                : GlobalId.CreateGlobalId(go.ClassId + 500, index);
        }

        private void RemoveGameObjectTotally(Game_Object go)
        {
            GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorkerBuilding())
                    Level.VillageWorkerManager.DecreaseWorkerCount();
            }
            else if (go.ClassId == 7)
            {
                var b = (Builder_Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorker2Building())
                    Level.BuilderWorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }
    }
}
