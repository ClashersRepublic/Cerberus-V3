using System.Collections.Generic;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Collect_Resource : Command
    {
        internal override int Type => 506;
        public Collect_Resource(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int BuildingId;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            Building Building = (Building)Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);

            if (Building != null)
            {
                if (Globals.CollectAllResourcesAtOnce)
                {
                    List<GameObject> Buildings = Level.GameObjectManager.Filter.GetGameObjectsByData(Building.Data);

                    Buildings.ForEach(GameObject =>
                    {
                        Building building = (Building)GameObject;

                        if (!building.Constructing)
                        {
                            ResourceProductionComponent ResourceProductionComponent = building.ResourceProductionComponent;

                            ResourceProductionComponent?.CollectResources();
                        }
                    });
                }
                else
                {
                    ResourceProductionComponent ResourceProductionComponent = Building.ResourceProductionComponent;

                    ResourceProductionComponent?.CollectResources();
                }
            }
        }
    }
}
