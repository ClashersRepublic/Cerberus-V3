using System;
using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class GameObjectManager
    {
        internal Level Level;

        internal List<GameObject>[][] GameObjects;

        internal Building Bunker;
        internal Building TownHall;
        internal Building TownHall2;
        internal Building Laboratory;

        internal Filter Filter;
        internal Random Random;

        internal int SecondsFromLastRespawn;
        internal int ObstacleClearCounter;

        internal int Checksum
        {
            get
            {
                var Checksum = 0;

                for (var i = 0; i < 9; i++)
                {
                    Checksum += this.GameObjects[i][0].Count;
                    Checksum += this.GameObjects[i][1].Count;
                }

                int c = 0;
                foreach (var gameObject in this.GameObjects)
                {
                    for (var j = 0; j < gameObject[0].Count; j++)
                    {
                        Checksum += gameObject[0][j].Checksum;
                    }

                    for (var j = 0; j < gameObject[1].Count; j++)
                    {
                        Checksum += gameObject[1][j].Checksum;
                    }
                    c++;
                }

                return Checksum;
            }
        }

        internal int Map => Level.Player?.Map ?? 0;

        public GameObjectManager()
        {
            this.GameObjects = new List<GameObject>[10][];

            for (var i = 0; i < this.GameObjects.Length; i++)
            {
                this.GameObjects[i] = new List<GameObject>[2];

                for (var j = 0; j < this.GameObjects[i].Length; j++)
                {
                    this.GameObjects[i][j] = new List<GameObject>();
                }
            }

            this.Filter = new Filter(this);
            this.Random = new Random();
        }

        public GameObjectManager(Level Level) : this()
        {
            this.Level = Level;
        }

        internal void AddGameObject(GameObject GameObject, int Map)
        {
            int GType = GameObject.Type;

            if (GType == 0)
            {
                Building Building = (Building)GameObject;
                BuildingData Data = Building.BuildingData;

                if (Data.IsTownHall)
                {
                    this.TownHall = Building;
                }

                if (Data.IsTownHall2)
                {
                    this.TownHall2 = Building;
                }

                if (Data.IsWorker)
                {
                    this.Level.WorkerManager.WorkerCount++;
                }
            }
            else
            {
                //Some shit
            }

            GameObject.Id = GlobalId.Create(500 + GType, this.GameObjects[GType][Map].Count);

            this.GameObjects[GType][Map].Add(GameObject);
            this.Level.TileMap.AddGameObject(GameObject);
        }

        internal void CreateObstacle()
        {
            List<Data> Tables = CSV.Tables.Get(Gamefile.Obstacles).Datas;

            if (Tables.Count < 1)
            {
                return;
            }

            int Weight = 0;

            foreach (ObstacleData Data in Tables)
            {
                if (Data.VillageType == this.Map)
                {
                    Weight += Data.RespawnWeight;
                }
            }

            int RandomWeight = this.Random.Rand(Weight);

            Weight = 0;

            foreach (ObstacleData Data in Tables)
            {
                if (Data.VillageType == this.Map)
                {
                    Weight += Data.RespawnWeight;

                    if (Weight > RandomWeight)
                    {
                        this.RandomlyPlaceObstacle(Data);
                        break;
                    }
                }
            }
        }

        internal void RandomlyPlaceObstacle(ObstacleData Data)
        {
            if (Data.VillageType == this.Map)
            {
                int WidthInTiles = this.Level.WidthInTiles;
                int HeightInTiles = this.Level.HeightInTiles;

                for (int i = 0; i <= 20; i++)
                {
                    int X = this.Random.Rand(WidthInTiles + 1 - Data.Width);
                    int Y = this.Random.Rand(HeightInTiles + 1 - Data.Height);

                    if (this.Level.IsValidPlaceForObstacle(Data, X, Y, Data.Width, Data.Height, true))
                    {
                        Obstacle Obstacle = (Obstacle)GameObjectFactory.CreateGameObject(Data, this.Level);
                        Obstacle.SetPositionXY(X, Y);
                        this.AddGameObject(Obstacle, this.Map);

                        //Logging.Info(this.GetType(), "X:" + X + "   Y:" + Y);

                        break;
                    }
                }
            }
            //else
               // Logging.Error(this.GetType(), "RandomlyPlaceObstacle() - Trying to place obstacle in wrong village");
        }

        internal void FastForwardTime(int Secs)
        {
            for (int i = 0; i < this.GameObjects.Length; i++)
            {
                for (int j = 0; j < this.GameObjects[i][0].Count; j++)
                {
                    this.GameObjects[i][0][j].FastForwardTime(Secs);
                }

                for (int j = 0; j < this.GameObjects[i][1].Count; j++)
                {
                    this.GameObjects[i][1][j].FastForwardTime(Secs);
                }
            }

            this.SecondsFromLastRespawn += Secs;

            this.Random.Rand(1);
            this.RespawnObstacles();
        }

        internal void RecalculateIds()
        {
            for (int i = 0; i < this.GameObjects[3][0].Count; i++)
            {
                Obstacle Obstacle = (Obstacle)this.GameObjects[3][0][i];

                if (Obstacle.Destructed)
                {
                    this.GameObjects[3][0].Remove(Obstacle);
                }
            }

            for (int i = 0; i < this.GameObjects[3][1].Count; i++)
            {
                Obstacle Obstacle = (Obstacle)this.GameObjects[3][1][i];

                if (Obstacle.Destructed)
                {
                    this.GameObjects[3][1].Remove(Obstacle);
                }
            }
        }

        internal void Process()
        {
            this.GameObjects[0][0].ForEach(GameObject =>
            {
                GameObject.Process();
            });

            this.GameObjects[0][1].ForEach(GameObject =>
            {
                GameObject.Process();
            });

            this.GameObjects[4][0].ForEach(GameObject =>
            {
                GameObject.Process();
            });

            this.GameObjects[4][1].ForEach(GameObject =>
            {
                GameObject.Process();
            });
        }

        internal void RespawnObstacles()
        {
            int TombStoneCount = this.Level.TombStoneCount;

            while (this.SecondsFromLastRespawn > Globals.ObstacleRespawnSeconds)
            {
                if (this.GameObjects[3][0].Count - TombStoneCount >= Globals.ObstacleCountMax)
                {
                    this.SecondsFromLastRespawn = 0;
                    break;
                }

                this.CreateObstacle();
                this.SecondsFromLastRespawn -= Globals.ObstacleRespawnSeconds;
            }
        }

        internal void Load(JToken Json)
        {
            #region Village 1

            JArray Buildings = (JArray)Json["buildings"];

            if (Buildings != null)
            {
                foreach (JToken Token in Buildings)
                {
                    this.LoadGameObject(Token, 0);
                }
            }
            //else
                //Logging.Error(this.GetType(), "An error has been throwed the load of the game objects. Building array is NULL!");

            JArray Obstacles = (JArray)Json["obstacles"];

            if (Obstacles != null)
            {
                foreach (JToken Token in Obstacles)
                {
                    this.LoadGameObject(Token, 0);
                }
            }

            JArray Traps = (JArray)Json["traps"];

            if (Traps != null)
            {
                foreach (JToken Token in Traps)
                {
                    this.LoadGameObject(Token, 0);
                }
            }

            JArray Decos = (JArray)Json["decos"];

            if (Decos != null)
            {
                foreach (JToken Token in Decos)
                {
                    this.LoadGameObject(Token, 0);
                }
            }

            #endregion
            #region Village 2

            JArray Buildings2 = (JArray)Json["buildings2"];

            if (Buildings2 != null)
            {
                foreach (JToken Token in Buildings2)
                {
                    this.LoadGameObject(Token, 1);
                }
            }

            JArray Obstacles2 = (JArray)Json["obstacles2"];


            if (Obstacles2 != null)
            {
                foreach (JToken Token in Obstacles2)
                {
                    this.LoadGameObject(Token, 1);
                }
            }

            JArray Traps2 = (JArray)Json["traps2"];

            if (Traps2 != null)
            {
                foreach (JToken Token in Traps2)
                {
                    this.LoadGameObject(Token, 1);
                }
            }

            JArray Decos2 = (JArray)Json["decos2"];

            if (Decos2 != null)
            {
                foreach (JToken Token in Decos2)
                {
                    this.LoadGameObject(Token, 1);
                }
            }

            #endregion

            if (JsonHelper.GetJsonObject(Json, "respawnVars", out JToken RespawnToken))
            {
                JsonHelper.GetJsonNumber(RespawnToken, "secondsFromLastRespawn", out this.SecondsFromLastRespawn);
                JsonHelper.GetJsonNumber(RespawnToken, "obstacleClearCounter", out this.ObstacleClearCounter);

                this.Random.Seed = JsonHelper.GetJsonNumber(RespawnToken, "respawnSeed", out int RandomSeed) ? RandomSeed : 112;
            }
            else
            {
                //Logging.Info(this.GetType(), "Load() - Can't find respawn variables.");
                this.Random.Seed = 112;
            }
        }


        internal void Save(JObject Json)
        {
            #region Village 1

            JArray Buildings = new JArray();

            foreach (GameObject GameObject in this.GameObjects[0][0])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Buildings.Add(Token);
                }
            }

            JArray Obstacles = new JArray();

            foreach (GameObject GameObject in this.GameObjects[3][0])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Obstacles.Add(Token);
                }
            }

            JArray Traps = new JArray();

            foreach (GameObject GameObject in this.GameObjects[4][0])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Traps.Add(Token);
                }
            }

            JArray Decos = new JArray();

            foreach (GameObject GameObject in this.GameObjects[6][0])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Decos.Add(Token);
                }
            }

            #endregion
            #region Village 2

            JArray Buildings2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[0][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Buildings2.Add(Token);
                }
            }

            JArray Obstacles2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[3][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Obstacles2.Add(Token);
                }
            }

            JArray Traps2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[4][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Traps2.Add(Token);
                }
            }

            JArray Decos2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[6][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);

                    GameObject.Save(Token);
                    Decos2.Add(Token);
                }
            }
        
            #endregion

            var RespawnVars = new JObject
            {
                {"secondsFromLastRespawn", this.SecondsFromLastRespawn},
                {"obstacleClearCounter", this.ObstacleClearCounter},
                {"respawnSeed", this.Random.Seed},
                {"time_to_gembox_drop", 999999999}
            };

            Json.Add("buildings", Buildings);
            Json.Add("obstacles", Obstacles);
            Json.Add("traps", Traps);
            Json.Add("decos", Decos);
            //Json.Add("vobjs", Decos2);
            Json.Add("buildings2", Buildings2);
            Json.Add("obstacles2", Obstacles2);
            Json.Add("traps2", Traps2);
            Json.Add("decos2", Decos2);
            //Json.Add("vobjs2", Decos2);
            Json.Add("respawnVars", RespawnVars);
        }


        internal void LoadGameObject(JToken Token, int Map)
        {
            if (JsonHelper.GetJsonNumber(Token, "data", out int DataID))
            {
                Data Data = CSV.Tables.GetWithGlobalId(DataID);

                if (Data == null)
                {
                    Logging.Error(this.GetType(), "An error has been throwed when the load of GameObject. data is not valid.");
                    return;
                }

                GameObject GameObject = GameObjectFactory.CreateGameObject(Data, this.Level);

                if (GameObject != null)
                {
                    GameObject.Load(Token);
                    this.AddGameObject(GameObject, Map);
                }
            }
            else
                Logging.Error(this.GetType(), "An error has been throwed when the load of GameObject. data not exist.");
        }

        internal void Tick()
        {
            foreach (List<GameObject>[] gameobject in this.GameObjects)
            {
                for (int j = 0; j < gameobject[0].Count; j++)
                {
                    gameobject[0][j].Tick();
                }

                for (int j = 0; j < gameobject[1].Count; j++)
                {
                    gameobject[1][j].Tick();
                }
            }
        }
    }
}
