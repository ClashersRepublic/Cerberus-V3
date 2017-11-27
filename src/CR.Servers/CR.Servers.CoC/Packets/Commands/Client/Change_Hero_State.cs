using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Change_Hero_State : Command
    {
        internal override int Type => 529;

        public Change_Hero_State(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int BuildingId;
        internal bool Sleeping;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Sleeping = this.Reader.ReadBoolean();
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
                        var HeroData = HeroBaseComponent.HeroData;
                        if (HeroData != null)
                        {
                            level.Player.HeroStates.Set(HeroData, this.Sleeping ? 2 : 3);
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to change hero state. Hero data is null.");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to change hero state. The HeroBaseComponent is null.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to change hero state. The game object is not a building.");
            }
            else
                Logging.Error(this.GetType(), "Unable to change hero state. The game object is null.");
        }
    }
}
