using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Core.Consoles.Colorful;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Building : Command
    {
        internal override int Type => 502;

        public Upgrade_Building(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int Id;
        internal bool UseAltResource;

        internal override void Decode()
        {
            this.Id = Reader.ReadInt32();
            this.UseAltResource = Reader.ReadBoolean();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = Device.GameMode.Level;
            var GameObject = Level.GameObjectManager.Filter.GetGameObjectById(this.Id);
            if (GameObject != null)
            {
                if (GameObject.Type == 0)
                {
                    var Building = GameObject as Building;
                    if (Building != null)
                    {
                        if (Building.UpgradeAvailable)
                        {
                            BuildingData Data = (BuildingData) Building.Data;
                            ResourceData ResourceData = this.UseAltResource
                                ? Data.AltBuildResourceData
                                : Data.BuildResourceData;

                            if (ResourceData != null)
                            {
                                Console.WriteLine(Data.BuildCost[Building.GetUpgradeLevel() + 1]);
                                if (Level.Player.Resources.GetCountByData(ResourceData) >=
                                    Data.BuildCost[Building.GetUpgradeLevel() + 1])
                                {
                                    if (Level.GameObjectManager.Map == 0
                                        ? Level.WorkerManager.FreeWorkers > 0
                                        : Level.WorkerManagerV2.FreeWorkers > 0)
                                    {
                                        Level.Player.Resources.Remove(ResourceData,
                                            Data.BuildCost[Building.GetUpgradeLevel() + 1]);
                                        Building.StartUpgrade();

                                        if (Data.IsTownHall2)
                                        {
                                            if (Level.Player.TownHallLevel2 == 0)
                                                Parallel.ForEach(Level.GameObjectManager.GameObjects[0][1].ToArray(),
                                                    Object =>
                                                    {
                                                        var b2 = (Building) Object;
                                                        var bd2 = b2.BuildingData;
                                                        if (b2.Locked)
                                                        {
                                                            if (bd2.Locked)
                                                                return;
#if DEBUG
                                                            Logging.Info(this.GetType(),
                                                                $"Builder Building: Unlocking {bd2.Name} with ID {Object.Id}");
#endif
                                                            b2.Locked = false;
                                                        }
                                                    });

                                            Level.Player.TownHallLevel2++;
                                        }
                                        else if (Data.IsAllianceCastle)
                                        {

                                            Level.Player.CastleLevel++;
                                            Level.Player.CastleTotalCapacity =
                                                Data.HousingSpace[Level.Player.CastleLevel];
                                            Level.Player.CastleTotalSpellCapacity =
                                                Data.HousingSpaceAlt[Level.Player.CastleLevel];
                                        }
                                        else if (Data.IsTownHall)
                                        {
                                            Level.Player.TownHallLevel++;
                                        }
                                    }
                                }
                                else
                                    Logging.Error(this.GetType(),
                                        "Unable to upgrade the building. You doesn't have enough resources.");
                            }
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to upgrade the building. Upgrade is not available.");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to upgrade the building. GameObject is not valid or not exist.");
                }
                else if (GameObject.Type == 4)
                {
                    //Trap
                }
                else if (GameObject.Type == 8)
                {
                    var Object = GameObject as VillageObject;
                    if (Object != null)
                    {
                        VillageObjectData Data = (VillageObjectData) Object.Data;
                        ResourceData ResourceData = Data.BuildResourceData;

                        if (ResourceData != null)
                        {
                            if (Level.Player.Resources.GetCountByData(ResourceData) >=
                                Data.BuildCost[Object.GetUpgradeLevel() + 1])
                            {
                                if (Level.GameObjectManager.Map == 0
                                    ? Level.WorkerManager.FreeWorkers > 0
                                    : Level.WorkerManagerV2.FreeWorkers > 0)
                                {
                                    Level.Player.Resources.Remove(ResourceData,
                                        Data.BuildCost[Object.GetUpgradeLevel() + 1]);
                                    Object.StartUpgrade();
                                }
                            }
                            else
                                Logging.Error(this.GetType(),
                                    "Unable to upgrade the VillageObject. You doesn't have enough resources.");
                        }
                    }
                    else
                        Logging.Error(this.GetType(),
                            "Unable to upgrade the VillageObject. GameObject is not valid or not exist.");
                }
                else
                    Logging.Error(this.GetType(),
                        $"Unable to determined Game Object type. Game Object type {GameObject.Type}.");
            }
            else
                Logging.Error(this.GetType(),
                    "Unable to upgrade the gameObject. GameObject is null");
        }
        
    }
}
