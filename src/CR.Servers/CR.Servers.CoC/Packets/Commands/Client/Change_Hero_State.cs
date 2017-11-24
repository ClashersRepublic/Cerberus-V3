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
                        var HeroData = HeroBaseComponent.HeroData;
                        if (HeroData != null)
                        {
                            var state = level.Player.HeroStates.GetCountByData(HeroData);
                            switch (state)
                            {
                                case 3:
                                    level.Player.HeroStates.Set(HeroData, 2);
                                    break;
                                case 2:
                                    level.Player.HeroStates.Set(HeroData, 3);
                                    break;
                                default:
                                    Logging.Error(this.GetType(), $"Unable to change hero state. Hero current state is {state}.");
                                    break;
                            }
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
