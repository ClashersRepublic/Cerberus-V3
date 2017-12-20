using System;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
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
            GameObject GameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(this.Id);

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
                                            this.Device.GameMode.Level.GameObjectManager.RemoveGameObject(TileObject, 1);
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
        }
    }
}
