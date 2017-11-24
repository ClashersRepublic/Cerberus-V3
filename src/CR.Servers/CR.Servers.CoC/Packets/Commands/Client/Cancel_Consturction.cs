using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Cancel_Consturction : Command
    {
        internal override int Type => 505;

        public Cancel_Consturction(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int BuildingID;

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            var GameObject = Level.GameObjectManager.Filter.GetGameObjectByPreciseId(this.BuildingID);

            if (GameObject != null)
            {
                if (GameObject is Building Building)
                {
                    if (!Building.Constructing)
                    {
                        Logging.Error(this.GetType(), $"Tried to cancel the construction of a building which is not in construction with game ID {this.BuildingID}.");
                    }
                    else
                    {
                        Building.CancelConstruction();
                    }
                }
                else if (GameObject is Trap Trap)
                {
                    if (!Trap.Constructing)
                    {
                        Logging.Error(this.GetType(), $"Tried to cancel the construction of a trap which is not in construction with game ID {this.BuildingID}.");
                    }
                    else
                    {
                        Trap.CancelConstruction();
                    }
                }
                else if (GameObject is Obstacle Obstacle)
                {
                    if (!Obstacle.ClearingOnGoing)
                    {
                        Logging.Error(this.GetType(), $"Tried to cancel the clearing of an obstacle which is not in clearing with game ID {this.BuildingID}.");
                    }
                    else
                    {
                        Obstacle.CancelClearing();
                    }
                }
            }
        }
    }
}
