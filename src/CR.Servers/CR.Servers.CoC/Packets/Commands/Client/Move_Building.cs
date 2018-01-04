using System;
using System.Text;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Move_Building : Command
    {

        internal override int Type => 501;

        public Move_Building(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int X;
        internal int Y;

        internal int Id;

        internal override void Decode()
        {
            this.X = Reader.ReadInt32();
            this.Y = Reader.ReadInt32();
            this.Id = Reader.ReadInt32();

            base.Decode();
        }

        internal override void Execute()
        {
            GameObject GameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectByPreciseId(this.Id);

            if (GameObject != null)
            {
                if (GameObject.Type != 3)
                {
                    GameObject.SetPositionXY(this.X, this.Y);

                    if (GameObject.VillageType == 1)
                    {
                        var Tiles = this.Device.GameMode.Level.TileMap.GetTile(GameObject, this.X, this.Y, 1);
                        if (Tiles != null)
                        {
                            foreach (var Tile in Tiles)
                            {
                                foreach (var TileObject in Tile.GameObjects)
                                {
                                    if (TileObject is Obstacle Obstacle)
                                    {
                                        if (Obstacle.ObstacleData.TallGrass)
                                        {
                                            Obstacle.Destructed = true;
                                            this.Device.GameMode.Level.GameObjectManager
                                                .RemoveGameObject(TileObject, 1);
                                        }
                                    }
                                }
                            }
                        }
                        else
                            Logging.Error(this.GetType(), "Unexpected issue while moving building! Tiles is null");
                    }
                }
            }
            else
            {
                var Error = new StringBuilder();
                var Precise = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectByPreciseId(this.Id);
                if (Precise != null)
                {
                    Error.AppendLine($"Building Id :  {this.Id}");
                    Error.AppendLine($"Building Name :  {Precise.Data.Name}");
                    Error.AppendLine($"Building New X :  {this.X}");
                    Error.AppendLine($"Building New Y :  {this.Y}");
                    Error.AppendLine($"Building still null with precise id : {false}");

                    Error.AppendLine($"Player Id :  {this.Device.GameMode.Level.Player.UserId}");
                    Error.AppendLine($"Player current village :  {this.Device.GameMode.Level.GameObjectManager.Map}");
                    Error.AppendLine($"Player town hall level :  {this.Device.GameMode.Level.Player.TownHallLevel}");
                    Error.AppendLine($"Player town hall2 level :  {this.Device.GameMode.Level.Player.TownHallLevel2}");
                }
                else
                {
                    Error.AppendLine($"Precise building : null");

                    Error.AppendLine($"Building Id :  {this.Id}");
                    Error.AppendLine($"Building New X :  {this.X}");
                    Error.AppendLine($"Building New Y :  {this.Y}");

                    Error.AppendLine($"Player Id :  {this.Device.GameMode.Level.Player.UserId}");
                    Error.AppendLine($"Player current village :  {this.Device.GameMode.Level.GameObjectManager.Map}");
                    Error.AppendLine($"Player town hall level :  {this.Device.GameMode.Level.Player.TownHallLevel}");
                    Error.AppendLine($"Player town hall2 level :  {this.Device.GameMode.Level.Player.TownHallLevel2}");
                }

                Resources.Logger.Debug(Error);

            }
        }
    }
}
