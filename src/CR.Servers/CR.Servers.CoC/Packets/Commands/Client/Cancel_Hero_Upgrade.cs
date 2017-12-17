using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Cancel_Hero_Upgrade : Command
    {
        internal override int Type => 531;

        public Cancel_Hero_Upgrade(Device Device, Reader Reader) : base(Device, Reader)
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
            var level = this.Device.GameMode.Level;
            var gameObject = level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);
            if (gameObject != null)
            {
                if (gameObject is Building building)
                {
                    var HeroBaseComponent = building.HeroBaseComponent;
                    if (HeroBaseComponent != null)
                    {
                        if (!HeroBaseComponent.Upgrading)
                        {
                            Logging.Error(this.GetType(),  $"Tried to cancel the upgrade of a hero which is not in upgrading with game ID {this.BuildingId}.");
                        }
                        else
                        {
                            HeroBaseComponent.CancelUpgrade();
                        }
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to change hero mode. The HeroBaseComponent is null.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to change hero mode. The game object is not a building.");
            }
            else
                Logging.Error(this.GetType(), "Unable to change hero mode. The game object is null.");
        }
    }
}
