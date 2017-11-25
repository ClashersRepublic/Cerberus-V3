using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Move_Multiple_Buildings : Command
    {
        internal override int Type => 533;

        public Move_Multiple_Buildings(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal List<BuildingToMove> Buildings;

        internal override void Decode()
        {
            var buildingCount = Reader.ReadInt32();
            Buildings = new List<BuildingToMove>(buildingCount);

            for (var i = 0; i < buildingCount; i++)
            {
                Buildings.Add(new BuildingToMove
                {
                    X = Reader.ReadInt32(),
                    Y = Reader.ReadInt32(),
                    Id = Reader.ReadInt32()
                });
            }

            base.Decode();
        }

        internal override void Execute()
        {
            foreach (var building in Buildings)
            {
                GameObject GameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(building.Id);

                if (GameObject != null)
                {
                    if (GameObject.Type != 3)
                    {
                        GameObject.SetPositionXY(building.X, building.Y);
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to move the building. The game object is an obstacle");
                }
                else
                    Logging.Error(this.GetType(), "Unable to move the building. The game object is null") ;
            }
        }
    }
}
