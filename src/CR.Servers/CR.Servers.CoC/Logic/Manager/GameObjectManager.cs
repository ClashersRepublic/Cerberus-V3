namespace CR.Servers.CoC.Logic
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Extensions.Game;
    using CR.Servers.CoC.Extensions.Helper;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Helpers;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using Newtonsoft.Json.Linq;

    internal class GameObjectManager
    {
        internal Building Bunker;
        internal int[] DecoIndex;

        internal Filter Filter;

        internal List<GameObject>[][] GameObjects;
        internal Building Laboratory;
        internal Level Level;
        internal int ObstacleClearCounter;
        internal int ObstacleClearCounterV2;
        internal int[] ObstaclesIndex;

        internal int SecondsFromLastRespawn;

        internal int SecondsFromLastRespawnV2;

        internal int SecondsFromLastTgRespawn;
        internal Random TgRandom;
        internal Building TownHall;
        internal Building TownHall2;
        internal Random V2Random;
        internal Random VRandom;

        public GameObjectManager()
        {
            this.GameObjects = new List<GameObject>[10][];
            this.ObstaclesIndex = new int[2];
            this.DecoIndex = new int[2];

            for (int i = 0; i < this.GameObjects.Length; i++)
            {
                this.GameObjects[i] = new List<GameObject>[2];

                for (int j = 0; j < this.GameObjects[i].Length; j++)
                {
                    this.GameObjects[i][j] = new List<GameObject>();
                }
            }

            this.Filter = new Filter(this);
            this.VRandom = new Random();
            this.V2Random = new Random();
            this.TgRandom = new Random();
        }

        public GameObjectManager(Level Level) : this()
        {
            this.Level = Level;
        }


        internal int Checksum
        {
            get
            {
                int Checksum = 0;

                for (int i = 0; i < 9; i++)
                {
                    Checksum += this.GameObjects[i][0].Count;
                    Checksum += this.GameObjects[i][1].Count;
                }

                int c = 0;
                foreach (List<GameObject>[] gameObject in this.GameObjects)
                {
                    for (int j = 0; j < gameObject[0].Count; j++)
                    {
                        Checksum += gameObject[0][j].Checksum;
                    }

                    for (int j = 0; j < gameObject[1].Count; j++)
                    {
                        Checksum += gameObject[1][j].Checksum;
                    }
                    c++;
                }

                return Checksum;
            }
        }

        internal int Map => this.Level.Player?.Map ?? 0;

        internal void AddGameObject(GameObject GameObject)
        {
            int GType = GameObject.Type;

            if (GType == 0)
            {
                Building Building = (Building) GameObject;
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

                if (Data.IsWorker2)
                {
                    this.Level.WorkerManagerV2.WorkerCount++;
                }

                if (Data.IsAllianceCastle)
                {
                    this.Bunker = Building;
                }
            }

            GameObject.Id = GlobalId.Create(500 + GType, this.GameObjects[GType][GameObject.VillageType].Count);
            this.GameObjects[GType][GameObject.VillageType].Add(GameObject);
            this.Level.TileMap.AddGameObject(GameObject);
        }

        public void RemoveGameObject(GameObject go, int Map)
        {
            this.GameObjects[go.Type][Map].Remove(go);
        }

        internal void CreateTallGrass()
        {
            List<Data> Tables = CSV.Tables.Get(Gamefile.Obstacles).Datas;

            if (Tables.Count < 1)
            {
                return;
            }

            int Weight = 0;

            foreach (ObstacleData Data in Tables)
            {
                if (Data.VillageType == 1 && Data.TallGrass)
                {
                    Weight += Data.RespawnWeight;
                }
            }

            int RandomWeight = this.TgRandom.Rand(Weight);

            Weight = 0;

            foreach (ObstacleData Data in Tables)
            {
                if (Data.VillageType == 1 && Data.TallGrass)
                {
                    Weight += Data.RespawnWeight;

                    if (Weight > RandomWeight)
                    {
                        this.RandomlyPlaceObstacle(Data, this.TgRandom);
                        break;
                    }
                }
            }
        }

        internal void CreateObstacle(int Type)
        {
            List<Data> Tables = CSV.Tables.Get(Gamefile.Obstacles).Datas;

            if (Tables.Count < 1)
            {
                return;
            }

            int Weight = 0;

            foreach (ObstacleData Data in Tables)
            {
                if (Data.VillageType == Type)
                {
                    Weight += Data.RespawnWeight;
                }
            }

            int RandomWeight = Type == 0 ? this.VRandom.Rand(Weight) : this.V2Random.Rand(Weight);

            Weight = 0;

            foreach (ObstacleData Data in Tables)
            {
                if (Data.VillageType == Type)
                {
                    Weight += Data.RespawnWeight;

                    if (Weight > RandomWeight)
                    {
                        this.RandomlyPlaceObstacle(Data, Type == 0 ? this.VRandom : this.V2Random);
                        break;
                    }
                }
            }
        }

        internal void RandomlyPlaceObstacle(ObstacleData Data, Random Random)
        {
            int WidthInTiles = this.Level.WidthInTiles;
            int HeightInTiles = this.Level.HeightInTiles;

            for (int i = 0; i <= 20; i++)
            {
                int X = Random.Rand(WidthInTiles + 1 - Data.Width);
                int Y = Random.Rand(HeightInTiles + 1 - Data.Height);

                if (this.Level.IsValidPlaceForObstacle(Data, X, Y, Data.Width, Data.Height, true))
                {
                    Obstacle Obstacle = new Obstacle(Data, this.Level)
                    {
                        Position =
                        {
                            X = X << 9,
                            Y = Y << 9
                        },
                        Id = GlobalId.Create(503, this.ObstaclesIndex[Data.VillageType]++)
                    };

                    this.GameObjects[3][Data.VillageType].Add(Obstacle);
                    this.Level.TileMap.AddGameObject(Obstacle);

                    //Logging.Info(this.GetType(),"Village:" + Data.VillageType +"   X:" + X + "   Y:" + Y);

                    break;
                }
            }
        }

        internal void FastForwardTime(int Secs)
        {
            foreach (List<GameObject>[] gameObject in this.GameObjects)
            {
                for (int j = 0; j < gameObject[0].Count; j++)
                {
                    gameObject[0][j].FastForwardTime(Secs);
                }

                for (int j = 0; j < gameObject[1].Count; j++)
                {
                    gameObject[1][j].FastForwardTime(Secs);
                }
            }

            //TODO Make auto respawn work.. Create timer system?
            this.SecondsFromLastRespawn += Secs;
            this.SecondsFromLastRespawnV2 += Secs;
            this.SecondsFromLastTgRespawn += Secs;

            this.VRandom.Rand(1);
            this.V2Random.Rand(1);
            this.TgRandom.Rand(1);
            this.RespawnObstacles();
        }

        internal void RecalculateAllIds()
        {
            for (int i = 0; i < this.GameObjects[0][0].Count; i++)
            {
                Building Building = (Building) this.GameObjects[0][0][i];
                Building.Id = GlobalId.Create(500, i);
            }

            for (int i = 0; i < this.GameObjects[0][1].Count; i++)
            {
                Building Building = (Building) this.GameObjects[0][1][i];
                Building.Id = GlobalId.Create(500, i);
            }

            int obstacleIndex = 0;

            for (int i = 0; i < this.GameObjects[3][0].Count; i++)
            {
                Obstacle Obstacle = (Obstacle) this.GameObjects[3][0][i];

                if (Obstacle.Destructed)
                {
                    this.GameObjects[3][0].Remove(Obstacle);
                }
                else
                {
                    Obstacle.Id = GlobalId.Create(503, obstacleIndex++);
                }
            }
            int obstacle2Index = 0;

            for (int i = 0; i < this.GameObjects[3][1].Count; i++)
            {
                Obstacle Obstacle = (Obstacle) this.GameObjects[3][1][i];

                if (Obstacle.Destructed)
                {
                    this.GameObjects[3][1].Remove(Obstacle);
                }
                else
                {
                    Obstacle.Id = GlobalId.Create(503, obstacle2Index++);
                }
            }

            //Trap
            //Deco
            //Vobjs
        }

        internal void RecalculateObstacleIds()
        {
            int obstacleIndex = 0;

            for (int i = 0; i < this.GameObjects[3][0].Count; i++)
            {
                Obstacle Obstacle = (Obstacle) this.GameObjects[3][0][i];

                if (Obstacle.Destructed)
                {
                    this.GameObjects[3][0].Remove(Obstacle);
                }
                else
                {
                    Obstacle.Id = GlobalId.Create(503, obstacleIndex++);
                }
            }

            int obstacle2Index = 0;

            for (int i = 0; i < this.GameObjects[3][1].Count; i++)
            {
                Obstacle Obstacle = (Obstacle) this.GameObjects[3][1][i];

                if (Obstacle.Destructed)
                {
                    this.GameObjects[3][1].Remove(Obstacle);
                }
                else
                {
                    Obstacle.Id = GlobalId.Create(503, obstacle2Index++);
                }
            }
        }

        internal void Process()
        {
            this.GameObjects[0][0].ForEach(GameObject => { GameObject.Process(); });

            this.GameObjects[0][1].ForEach(GameObject => { GameObject.Process(); });

            this.GameObjects[4][0].ForEach(GameObject => { GameObject.Process(); });

            this.GameObjects[4][1].ForEach(GameObject => { GameObject.Process(); });
        }

        internal void RespawnObstacles()
        {
            int TombStoneCount = this.Level.TombStoneCount;
            int TombStoneV2Count = this.Level.TombStoneV2Count;

            while (this.SecondsFromLastRespawn >= Globals.ObstacleRespawnSeconds)
            {
                if (this.GameObjects[3][0].Count - TombStoneCount >= Globals.ObstacleCountMax)
                {
                    this.SecondsFromLastRespawn = 0;
                    break;
                }

                this.CreateObstacle(0);
                this.SecondsFromLastRespawn -= Globals.ObstacleRespawnSeconds;
            }

            while (this.SecondsFromLastRespawnV2 >= Globals.ObstacleRespawnSeconds)
            {
                if (this.GameObjects[3][1].Count - TombStoneV2Count >= Globals.ObstacleCountMax)
                {
                    this.SecondsFromLastRespawnV2 = 0;
                    break;
                }

                this.CreateObstacle(1);
                this.SecondsFromLastRespawnV2 -= Globals.ObstacleRespawnSeconds;
            }

            while (this.SecondsFromLastTgRespawn >= Globals.TallGrassRespawnSeconds)
            {
                if (this.GameObjects[3][1].Count - TombStoneV2Count >= Globals.ObstacleCountMax)
                {
                    this.SecondsFromLastTgRespawn = 0;
                    break;
                }

                this.CreateTallGrass();
                this.SecondsFromLastTgRespawn -= Globals.TallGrassRespawnSeconds;
            }
        }

        internal void Load(JToken Json)
        {
            #region Village 1

            JArray Buildings = (JArray) Json["buildings"];

            if (Buildings != null)
            {
                foreach (JToken Token in Buildings)
                {
                    this.LoadGameObject(Token);
                }
            }
            else
            {
                Logging.Error(this.GetType(), "An error has been throwed the load of the game objects. Building array is NULL!");
            }

            JArray Obstacles = (JArray) Json["obstacles"];

            if (Obstacles != null)
            {
                foreach (JToken Token in Obstacles)
                {
                    if (JsonHelper.GetJsonNumber(Token, "id", out int Id))
                    {
                        this.ObstaclesIndex[0] = Math.Max(this.ObstaclesIndex[0], Id % 1000000);
                    }

                    this.LoadGameObject(Token);
                }
            }

            JArray Traps = (JArray) Json["traps"];

            if (Traps != null)
            {
                foreach (JToken Token in Traps)
                {
                    this.LoadGameObject(Token);
                }
            }

            JArray Decos = (JArray) Json["decos"];

            if (Decos != null)
            {
                foreach (JToken Token in Decos)
                {
                    if (JsonHelper.GetJsonNumber(Token, "id", out int Id))
                    {
                        this.DecoIndex[0] = Math.Max(this.DecoIndex[0], Id % 1000000);
                    }

                    this.LoadGameObject(Token);
                }
            }

            JArray VillageObjects = (JArray) Json["vobjs"];

            if (VillageObjects != null)
            {
                foreach (JToken Token in VillageObjects)
                {
                    this.LoadGameObject(Token);
                }
            }

            #endregion

            #region Village 2

            JArray Buildings2 = (JArray) Json["buildings2"];

            if (Buildings2 != null)
            {
                foreach (JToken Token in Buildings2)
                {
                    this.LoadGameObject(Token);
                }
            }

            JArray Obstacles2 = (JArray) Json["obstacles2"];


            if (Obstacles2 != null)
            {
                foreach (JToken Token in Obstacles2)
                {
                    if (JsonHelper.GetJsonNumber(Token, "id", out int Id))
                    {
                        this.ObstaclesIndex[1] = Math.Max(this.ObstaclesIndex[1], Id % 1000000);
                    }

                    this.LoadGameObject(Token);
                }
            }

            JArray Traps2 = (JArray) Json["traps2"];

            if (Traps2 != null)
            {
                foreach (JToken Token in Traps2)
                {
                    this.LoadGameObject(Token);
                }
            }

            JArray Decos2 = (JArray) Json["decos2"];

            if (Decos2 != null)
            {
                foreach (JToken Token in Decos2)
                {
                    if (JsonHelper.GetJsonNumber(Token, "id", out int Id))
                    {
                        this.DecoIndex[1] = Math.Max(this.DecoIndex[1], Id % 1000000);
                    }

                    this.LoadGameObject(Token);
                }
            }

            JArray VillageObjects2 = (JArray) Json["vobjs2"];

            if (VillageObjects2 != null)
            {
                foreach (JToken Token in VillageObjects2)
                {
                    this.LoadGameObject(Token);
                }
            }

            #endregion

            if (JsonHelper.GetJsonObject(Json, "respawnVars", out JToken RespawnToken))
            {
                JsonHelper.GetJsonNumber(RespawnToken, "secondsFromLastRespawn", out this.SecondsFromLastRespawn);
                JsonHelper.GetJsonNumber(RespawnToken, "obstacleClearCounter", out this.ObstacleClearCounter);

                this.VRandom.Seed = JsonHelper.GetJsonNumber(RespawnToken, "respawnSeed", out int RandomSeed) ? RandomSeed : 112;
            }
            else
            {
                Logging.Info(this.GetType(), "Load() - Can't find respawn variables.");
                this.VRandom.Seed = 112;
            }

            this.SecondsFromLastRespawnV2 = JsonHelper.GetJsonNumber(Json, "v2rs", out int SecondsFromLastRespawnV2) ? SecondsFromLastRespawnV2 : 0;
            this.V2Random.Seed = JsonHelper.GetJsonNumber(Json, "v2rseed", out int V2RandomSeed) ? V2RandomSeed : 112;
            this.ObstacleClearCounterV2 = JsonHelper.GetJsonNumber(Json, "v2ccounter", out int ObstacleClearCounterV2) ? ObstacleClearCounterV2 : 0;

            this.SecondsFromLastTgRespawn = JsonHelper.GetJsonNumber(Json, "tgsec", out int SecondsFromLastTGRespawn) ? SecondsFromLastTGRespawn : 0;
            this.TgRandom.Seed = JsonHelper.GetJsonNumber(Json, "tgseed", out int TGRandomSed) ? TGRandomSed : 112;
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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

                    GameObject.Save(Token);
                    Decos.Add(Token);
                }
            }

            JArray VillageObjects = new JArray();

            foreach (GameObject GameObject in this.GameObjects[8][0])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);
                    Token.Add("id", GameObject.Id);

                    GameObject.Save(Token);
                    VillageObjects.Add(Token);
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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

                    GameObject.Save(Token);
                    Decos2.Add(Token);
                }
            }

            JArray VillageObjects2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[8][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);
                    Token.Add("id", GameObject.Id);

                    GameObject.Save(Token);
                    VillageObjects2.Add(Token);
                }
            }

            #endregion

            JObject RespawnVars = new JObject
            {
                {"secondsFromLastRespawn", this.SecondsFromLastRespawn},
                {"obstacleClearCounter", this.ObstacleClearCounter},
                {"respawnSeed", this.VRandom.Seed},
                {"time_to_gembox_drop", 999999999}
            };

            Json.Add("buildings", Buildings);
            Json.Add("obstacles", Obstacles);
            Json.Add("traps", Traps);
            Json.Add("decos", Decos);
            Json.Add("vobjs", VillageObjects);
            Json.Add("respawnVars", RespawnVars);
            Json.Add("buildings2", Buildings2);
            Json.Add("obstacles2", Obstacles2);
            Json.Add("traps2", Traps2);
            Json.Add("decos2", Decos2);
            Json.Add("vobjs2", VillageObjects2);
            Json.Add("v2rs", this.SecondsFromLastRespawnV2);
            Json.Add("v2rseed", this.V2Random.Seed);
            Json.Add("v2ccounter", this.ObstacleClearCounterV2);
            Json.Add("tgsec", this.SecondsFromLastTgRespawn);
            Json.Add("tgseed", this.TgRandom.Seed);
            Json.Add("last_news_seen", 999999999);
        }

        internal void SaveV2(JObject Json)
        {
            #region Village 2

            JArray Buildings2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[0][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

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
                    Token.Add("id", GameObject.Id);

                    GameObject.Save(Token);
                    Decos2.Add(Token);
                }
            }

            JArray VillageObjects2 = new JArray();

            foreach (GameObject GameObject in this.GameObjects[8][1])
            {
                JObject Token = new JObject();

                if (GameObject.Data != null)
                {
                    Token.Add("data", GameObject.Data.GlobalId);
                    Token.Add("id", GameObject.Id);

                    GameObject.Save(Token);
                    VillageObjects2.Add(Token);
                }
            }

            #endregion

            JObject RespawnVars = new JObject
            {
                {"secondsFromLastRespawn", this.SecondsFromLastRespawn},
                {"obstacleClearCounter", this.ObstacleClearCounter},
                {"respawnSeed", this.VRandom.Seed},
                {"time_to_gembox_drop", 999999999}
            };

            Json.Add("buildings", new JArray());
            Json.Add("obstacles", new JArray());
            Json.Add("traps", new JArray());
            Json.Add("decos", new JArray());
            Json.Add("vobjs", new JArray());

            Json.Add("respawnVars", RespawnVars);

            Json.Add("buildings2", Buildings2);
            Json.Add("obstacles2", Obstacles2);
            Json.Add("traps2", Traps2);
            Json.Add("decos2", Decos2);
            Json.Add("vobjs2", VillageObjects2);

            Json.Add("v2rs", 0);
            Json.Add("v2rseed", 0);
            Json.Add("v2ccounter", 0);
            Json.Add("tgsec", 0);
            Json.Add("tgseed", 0);

            Json.Add("cooldowns", new JArray());

            Json.Add("newShopBuildings", new JArray());
            Json.Add("newShopTraps", new JArray());
            Json.Add("newShopDecos", new JArray());

            Json.Add("last_news_seen", 999999999);
        }

        internal void LoadGameObject(JToken Token)
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
                    this.AddGameObject(GameObject);
                }
            }
            else
            {
                Logging.Error(this.GetType(), "An error has been throwed when the load of GameObject. data not exist.");
            }
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