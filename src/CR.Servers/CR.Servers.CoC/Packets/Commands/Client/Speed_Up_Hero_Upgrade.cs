using System;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Speed_Up_Hero_Upgrade : Command
    {
        internal override int Type => 528;

        public Speed_Up_Hero_Upgrade(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int BuildingId;
        internal int Village;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Village = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var gameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);

            if (gameObject != null)
            {
                if (gameObject is Building building)
                {
                    var HeroBaseComponent = building.HeroBaseComponent;

                    if (HeroBaseComponent != null)
                    {
                        HeroBaseComponent.SpeedUpUpgrade();
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to speed up the hero upgrade. The game object doesn't contain a UnitUpgradeComponent.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to speed up the hero upgrade. The game object is not a building.");
            }
            else
                Logging.Error(this.GetType(), "Unable to speed up the hero upgrade. The game object is null");
        }
    }
}
