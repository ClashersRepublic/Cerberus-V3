namespace CR.Servers.CoC.Logic
{
    using System;
    using CR.Servers.CoC.Extensions.Helper;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using CR.Servers.CoC.Logic.Manager;
    using CR.Servers.CoC.Logic.Map;
    using CR.Servers.CoC.Logic.Mode;
    using CR.Servers.Logic.Enums;
    using Newtonsoft.Json.Linq;

    internal class Level
    {
        internal bool AI;

        internal string[] ArmyNames = {"", "", "", ""};
        //internal CooldownManager CooldownManager;

        internal ComponentManager ComponentManager;

        internal bool EditModeShown;

        internal GameMode GameMode;

        internal GameObjectManager GameObjectManager;
        internal Home Home;

        internal int LastLeagueRank;
        internal int LastLeagueShuffleInfo;

        internal Player Player;
        internal SpellProductionManager SpellProductionManager;
        internal TileMap TileMap;

        internal string TroopRequestMessage = "I need reinforcements";
        internal UnitProductionManager UnitProductionManager;
        internal string WarRequestMessage = "I need reinforcements";
        internal WorkerManager WorkerManager;
        internal WorkerManagerV2 WorkerManagerV2;

        public Level()
        {
            this.GameObjectManager = new GameObjectManager(this);
            this.WorkerManager = new WorkerManager();
            this.WorkerManagerV2 = new WorkerManagerV2();
            this.ComponentManager = new ComponentManager(this);
            this.UnitProductionManager = new UnitProductionManager(this);
            this.SpellProductionManager = new SpellProductionManager(this);
            //this.CooldownManager = new CooldownManager(this);
            /*
            this.MissionManager = new MissionManager(this);*/

            this.TileMap = new TileMap(50, 50);
        }

        public Level(GameMode GameMode) : this()
        {
            this.GameMode = GameMode;
        }

        public Level(bool AI)
        {
            this.AI = AI;

            this.Player = new Player(null, 0, 0)
            {
                Name = "Clashers Republic - AI Base",
                NameSetByUser = true,
                Score = 9999,
                League = 22,
                ExpLevel = 300
            };

            this.Home = new Home();
        }

        internal int WidthInTiles
        {
            get
            {
                return 50;
            }
        }

        internal int HeightInTiles
        {
            get
            {
                return 50;
            }
        }

        internal Time Time
        {
            get
            {
                return this.GameMode.Time;
            }
        }

        internal State State
        {
            get
            {
                return this.GameMode.Device.State;
            }
        }

        internal int TombStoneCount
        {
            get
            {
                int Count = 0;

                this.GameObjectManager.GameObjects[3][0].ForEach(GameObject =>
                {
                    Obstacle Obstacle = (Obstacle) GameObject;

                    if (Obstacle.ObstacleData.IsTombstone)
                    {
                        ++Count;
                    }
                });

                return Count;
            }
        }

        internal int TombStoneV2Count
        {
            get
            {
                int Count = 0;

                this.GameObjectManager.GameObjects[3][1].ForEach(GameObject =>
                {
                    Obstacle Obstacle = (Obstacle) GameObject;

                    if (Obstacle.ObstacleData.IsTombstone)
                    {
                        ++Count;
                    }
                });

                return Count;
            }
        }

        internal bool IsBuildingCapReached(BuildingData Data)
        {
            int TownHallLevel = Data.VillageType == 0 ? this.GameObjectManager.TownHall.GetUpgradeLevel() : this.GameObjectManager.TownHall2.GetUpgradeLevel();
            TownhallLevelData LevelData = (TownhallLevelData) CSV.Tables.Get(Gamefile.Townhall_Levels).GetDataWithInstanceID(TownHallLevel + 1);
            return this.GameObjectManager.Filter.GetGameObjectCount(Data, -1) >= LevelData?.Caps[Data];
        }

        internal void FastForwardTime(int Seconds)
        {
            this.GameObjectManager.FastForwardTime(Seconds);
            this.UnitProductionManager.FastForwardTime(Seconds);
            this.SpellProductionManager.FastForwardTime(Seconds);
            //this.CooldownManager.FastForwardTime(Seconds);
        }

        internal void Process()
        {
            this.GameObjectManager.Process();
            //this.MissionManager.Process();
            this.ComponentManager.RefreshResourceCaps();
        }

        internal void SetGameMode(GameMode GameMode)
        {
            this.GameMode = GameMode;
        }

        internal void SetPlayer(Player Player)
        {
            this.Player = Player;
            this.Player.Level = this;
            this.Player.Process();
        }

        internal void SetHome(Home Home)
        {
            this.Home = Home;
            this.Home.Level = this;

            JToken Token = Home.LastSave;
            this.GameObjectManager.Load(Token);
            this.UnitProductionManager.Load(Token["units"]?["unit_prod"]);
            this.SpellProductionManager.Load(Token["spells"]?["unit_prod"]);
            //this.CooldownManager.Load(Token);

            JsonHelper.GetJsonNumber(Token, "last_league_rank", out this.LastLeagueRank);
            JsonHelper.GetJsonNumber(Token, "last_league_shuffle", out this.LastLeagueShuffleInfo);

            JsonHelper.GetJsonBoolean(Token, "edit_mode_shown", out this.EditModeShown);

            JsonHelper.GetJsonString(Token, "troop_req_msg", out this.TroopRequestMessage);
            JsonHelper.GetJsonString(Token, "war_req_msg", out this.WarRequestMessage);

            if (JsonHelper.GetJsonArray(Token, "army_names", out JArray Army))
            {
                this.ArmyNames = Army.ToObject<string[]>();
            }
        }

        internal JObject Battle()
        {
            JObject Json = new JObject {{"exp_ver", 1}};

            this.GameObjectManager.Save(Json);
            this.Player.Battle(Json);

            return Json;
        }

        internal JObject BattleV2()
        {
            JObject Json = new JObject
            {
                {"exp_ver", 1},
                {"direct2", true}
            };

            this.GameObjectManager.SaveV2(Json);
            this.Player.Battle(Json);

            return Json;
        }

        internal void Load(JObject Json)
        {
            this.GameObjectManager.Load(Json);
            //this.CooldownManager.Load(Token);
            this.UnitProductionManager.Load(Json["units"]?["unit_prod"]);
            this.SpellProductionManager.Load(Json["spells"]?["unit_prod"]);

            JsonHelper.GetJsonNumber(Json, "last_league_rank", out this.LastLeagueRank);
            JsonHelper.GetJsonNumber(Json, "last_league_shuffle", out this.LastLeagueShuffleInfo);
            JsonHelper.GetJsonBoolean(Json, "edit_mode_shown", out this.EditModeShown);
            JsonHelper.GetJsonString(Json, "troop_req_msg", out this.TroopRequestMessage);
            if (JsonHelper.GetJsonArray(Json, "army_names", out JArray Army))
            {
                this.ArmyNames = Army.ToObject<string[]>();
            }
        }

        internal JObject Save()
        {
            JObject Json = new JObject
            {
                {"exp_ver", 1}
            };

            this.GameObjectManager.Save(Json);
            //this.CooldownManager.Save(Json);

            Json.Add("units", new JObject
            {
                {
                    "unit_prod",
                    this.UnitProductionManager.Save()
                }
            });

            Json.Add("spells", new JObject
            {
                {
                    "unit_prod",
                    this.SpellProductionManager.Save()
                }
            });

            //Json.Add("newShopDecos", new JArray());

            Json.Add("last_league_rank", this.LastLeagueRank);
            Json.Add("last_league_shuffle", this.LastLeagueShuffleInfo);

            Json.Add("edit_mode_shown", this.EditModeShown);

            Json.Add("troop_req_msg", this.TroopRequestMessage);
            Json.Add("war_req_msg", this.WarRequestMessage);

            Json.Add("army_names", new JArray
            {
                this.ArmyNames
            });


            return Json;
        }

        internal void Tick()
        {
            this.Player.LastTick = DateTime.Now;
            this.GameObjectManager.Tick();
            this.UnitProductionManager.Tick();
            this.SpellProductionManager.Tick();
            this.Player.Inbox.Tick();
            // this.MissionManager.Tick();

            //this.CooldownManager.Update(this.Time);
        }

        internal bool IsValidPlaceForObstacle(ObstacleData Data, int X, int Y, int Width, int Height, bool Edge)
        {
            bool Valid = false;

            if (X >= 0 && Y >= 0)
            {
                if (Width + X <= 50 && Height + Y <= 50)
                {
                    if (Edge)
                    {
                        Width += 2;
                        Height += 2;
                        X--;
                        Y--;
                    }

                    Valid = true;

                    if (Width > 0 && Height > 0)
                    {
                        for (int i = 0; i < Width; i++)
                        {
                            for (int j = 0; j < Height; j++)
                            {
                                Tile Tile = this.TileMap[X + i, Y + j, Data.VillageType];

                                if (Tile != null)
                                {
                                    if (!Tile.IsBuildable())
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Valid;
        }

        internal bool IsValidPlaceForBuilding(ObstacleData Data, int X, int Y, int Width, int Height)
        {
            bool Valid = false;

            if (X >= 0 && Y >= 0)
            {
                if (Width + X <= 50 && Height + Y <= 50)
                {
                    Valid = true;

                    if (Width > 0 && Height > 0)
                    {
                        for (int i = 0; i < Width; i++)
                        {
                            for (int j = 0; j < Height; j++)
                            {
                                Tile Tile = this.TileMap[X + i, Y + j, Data.VillageType];

                                if (Tile != null)
                                {
                                    if (!Tile.IsBuildable())
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Valid;
        }

        internal bool IsValidPlaceForBuilding(BuildingData Data, int X, int Y, int Width, int Height)
        {
            bool Valid = false;

            if (X >= 0 && Y >= 0)
            {
                if (Width + X <= 50 && Height + Y <= 50)
                {
                    Valid = true;

                    if (Width > 0 && Height > 0)
                    {
                        for (int i = 0; i < Width; i++)
                        {
                            for (int j = 0; j < Height; j++)
                            {
                                Tile Tile = this.TileMap[X + i, Y + j, Data.VillageType];

                                if (Tile != null)
                                {
                                    if (!Tile.IsBuildable())
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Valid;
        }
    }
}