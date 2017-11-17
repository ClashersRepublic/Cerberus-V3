using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Unlock_Building : Command
    {
        internal override int Type => 520;

        public Unlock_Building(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int BuildingId;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            ExecuteSubTick = this.Reader.ReadInt32();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            Building Building = Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId) as Building;

            if (Building != null)
            {
                if (Building.Locked)
                {
                    BuildingData Data = Building.BuildingData;

                    if (Data.BuildCost[0] > 0)
                    {
                        ResourceData ResourceData = Data.BuildResourceData;
                        if (Level.Player.Resources.GetCountByData(ResourceData) >= Data.BuildCost[0])
                            Level.Player.Resources.Remove(ResourceData, Data.BuildCost[0]);
                    }

                    Building.Locked = false;
                    //Building.SetUpgradeLevel(Building.GetUpgradeLevel());
                }
            }
        }
    }
}