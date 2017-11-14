using System.Runtime.InteropServices.ComTypes;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Logic.Map;
using CR.Servers.CoC.Logic.Mode;
using CR.Servers.CoC.Logic.Mode.Enums;
using CR.Servers.Core.Consoles.Colorful;
using Newtonsoft.Json.Linq;
using Console = System.Console;

namespace CR.Servers.CoC.Logic
{
    internal class Level
    {
        internal Player Player;
        internal Home Home;

        internal GameMode GameMode;
        internal TileMap TileMap;


        internal GameObjectManager GameObjectManager;
        internal WorkerManager WorkerManager;
        internal WorkerManagerV2 WorkerManagerV2;
        internal ComponentManager ComponentManager;

        internal int WidthInTiles => 50;

        internal int HeightInTiles => 50;

        internal Time Time => this.GameMode.Time;
        internal State State => this.GameMode.State;

        internal int TombStoneCount
        {
            get
            {
                var Count = 0;

                this.GameObjectManager.GameObjects[3][0].ForEach(GameObject =>
                {
                    var Obstacle = (Obstacle)GameObject;

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
            var LevelData = (TownhallLevelData)CSV.Tables.Get(Gamefile.Townhall_Levels).GetDataWithInstanceID(this.GameObjectManager.TownHall.GetUpgradeLevel());

            return GameObjectManager.Filter.GetGameObjectCount(Data, -1) >= LevelData?.Caps[Data];
        }

        public Level(GameMode GameMode)
        {
            this.GameMode = GameMode;

            this.GameObjectManager = new GameObjectManager(this);
            this.WorkerManager = new WorkerManager();
            this.WorkerManagerV2 = new WorkerManagerV2();
            this.ComponentManager = new ComponentManager(this);
            /*
            this.CooldownManager = new CooldownManager();
            this.UnitProductionManager = new UnitProductionManager(this);

            this.MissionManager = new MissionManager(this);*/

            this.TileMap = new TileMap(50, 50);
        }
        internal void FastForwardTime(int Seconds)
        {
            this.GameObjectManager.FastForwardTime(Seconds);
            //this.CooldownManager.FastForwardTime(Seconds);
        }
        
        internal void Process()
        {
            this.GameObjectManager.Process();
            //this.MissionManager.Process();
            this.ComponentManager.RefreshResourceCaps();
        }

        internal void SetPlayer(Player Player)
        {
            this.Player = Player;
            this.Player.Level = this;
        }

        internal void SetHome(Home Home)
        {
            this.Home = Home;
            this.Home.Level = this;

            var Token = Home.LastSave;
            this.GameObjectManager.Load(Token);
            //this.CooldownManager.Load(Token);
            //this.UnitProductionManager.Load(Home.HomeJSON["units"]?["unit_prod"]);
        }

        internal JObject Battle()
        {
            var Json = new JObject();

            Json.Add("exp_ver", 1);

            this.GameObjectManager.Save(Json);
            this.Player.Save(Json);

            return Json;
        }

        internal void Load(JObject Json)
        {
            this.GameObjectManager.Load(Json);
            //this.CooldownManager.Load(Token);
            //this.UnitProductionManager.Load(Home.HomeJSON["units"]?["unit_prod"]);
        }
        internal JObject Save()
        {
            JObject Json = new JObject();

            Json.Add("exp_ver", 1);
            
            this.GameObjectManager.Save(Json);
            //this.CooldownManager.Save(Json);

            /*Json.Add("units", new JObject
            {
                {
                    "unit_prod",
                    this.UnitProductionManager.Save()
                }
            });*/

            return Json;
        }

        internal void Tick()
        {
            this.GameObjectManager.Tick();
           // this.MissionManager.Tick();

            //this.CooldownManager.Update(this.Time);
        }

        internal bool IsValidPlaceForObstacle(ObstacleData Data, int X, int Y, int Width, int Height, bool Edge)
        {
            var Valid = false;

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
                        for (var i = 0; i < Width; i++)
                        {
                            for (var j = 0; j < Height; j++)
                            {
                                var Tile = this.TileMap[X + i, Y + j, Data.VillageType];

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
