using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Gear_Up : Command
    {
        internal override int Type => 600;

        public Gear_Up(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            base.Decode();
        }

        internal int BuildingID;

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            var GameObject = Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingID);

            if (GameObject != null)
            {
                if (GameObject is Building Building)
                {
                    BuildingData Data = (BuildingData) Building.Data;
                    ResourceData ResourceData = Data.GearUpResourceData;
                    if (ResourceData != null)
                    {
                        if (Level.Player.Resources.GetCountByData(ResourceData) >= Data.GearUpCost[Building.GetUpgradeLevel()])
                        {
                            if (Level.WorkerManagerV2.FreeWorkers > 0)
                            {
                                Level.Player.Resources.Remove(ResourceData,  Data.GearUpCost[Building.GetUpgradeLevel()]);
                                Building.StartGearing();
                            }
                        }
                        else
                            Logging.Error(this.GetType(),
                                "Unable to upstart the gear up. The player doesn't have enough resources.");
                    }
                    else
                        Logging.Error(this.GetType(),
                            "Unable to start the gear up. The resources data is null.");
                }
                else
                    Logging.Error(this.GetType(),
                        "Unable to start the gear up. GameObject is not valid or not exist.");
            }
            else
                Logging.Error(this.GetType(),
                    "Unable to start the gear up. GameObject is null");
        }
    }
}
