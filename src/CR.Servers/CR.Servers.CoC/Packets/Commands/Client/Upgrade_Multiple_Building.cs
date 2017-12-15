using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Upgrade_Multiple_Building : Command
    {
        internal override int Type => 549;

        public Upgrade_Multiple_Building(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal bool UseAltResource;
        internal int Count;
        internal List<int> BuildingIds;

        internal override void Decode()
        {
            this.UseAltResource = this.Reader.ReadBoolean();
            this.Count = this.Reader.ReadInt32();

            this.BuildingIds = new List<int>(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                this.BuildingIds.Add(this.Reader.ReadInt32());
            }

            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            foreach (var BuildingId in BuildingIds)
            {
                var GameObject = Level.GameObjectManager.Filter.GetGameObjectById(BuildingId);

                if (GameObject != null)
                {
                    if (GameObject is Building Building)
                    {
                        if (Building.UpgradeAvailable)
                        {
                            BuildingData Data = (BuildingData) Building.Data;
                            ResourceData ResourceData = this.UseAltResource ? Data.AltBuildResourceData(Building.GetUpgradeLevel() + 1) : Data.BuildResourceData;
                            if (ResourceData != null)
                            {
                                if (Level.Player.Resources.GetCountByData(ResourceData) >= Data.BuildCost[Building.GetUpgradeLevel() + 1])
                                {
                                    if (Level.GameObjectManager.Map == 0 ? Level.WorkerManager.FreeWorkers > 0 : Level.WorkerManagerV2.FreeWorkers > 0)
                                    {
                                        Level.Player.Resources.Remove(ResourceData, Data.BuildCost[Building.GetUpgradeLevel() + 1]);
                                        Building.StartUpgrade();
                                    }
                                }
                            }
                            else
                                Logging.Error(this.GetType(), "Unable to upgrade the building. The resource data is null.");
                        }
                        //else
                            //Logging.Error(this.GetType(), "Unable to upgrade the building. UpgradeAvailable returened false.");
                    }
                    else
                    {
                        //TODO: Add logging system that will inform the dev
                        Logging.Error(this.GetType(),
                            $"UpgradeMultipleBuilding is expected to be use only with Building but it was called for {GameObject.Type}. Please inform the developer right away!");
                    }

                }
                else 
                    Logging.Error(this.GetType(), "Unable to upgrade the building. The gameobject is null.");
            }

        }
    }
}
