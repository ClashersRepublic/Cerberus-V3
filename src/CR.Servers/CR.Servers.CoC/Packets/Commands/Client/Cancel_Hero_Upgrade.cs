namespace CR.Servers.CoC.Packets.Commands.Client
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class Cancel_Hero_Upgrade : Command
    {
        internal int BuildingId;

        public Cancel_Hero_Upgrade(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override int Type
        {
            get
            {
                return 531;
            }
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            Level level = this.Device.GameMode.Level;
            GameObject gameObject = level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);
            if (gameObject != null)
            {
                if (gameObject is Building building)
                {
                    HeroBaseComponent HeroBaseComponent = building.HeroBaseComponent;
                    if (HeroBaseComponent != null)
                    {
                        if (!HeroBaseComponent.Upgrading)
                        {
                            Logging.Error(this.GetType(), $"Tried to cancel the upgrade of a hero which is not in upgrading with game ID {this.BuildingId}.");
                        }
                        else
                        {
                            HeroBaseComponent.CancelUpgrade();
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Unable to change hero mode. The HeroBaseComponent is null.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Unable to change hero mode. The game object is not a building.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Unable to change hero mode. The game object is null.");
            }
        }
    }
}