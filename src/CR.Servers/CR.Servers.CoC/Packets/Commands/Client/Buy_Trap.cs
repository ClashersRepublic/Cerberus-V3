using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Buy_Trap : Command
    {
        internal override int Type => 510;

        public Buy_Trap(Device device, Reader reader) : base(device, reader)
        {
        }


        internal int X;
        internal int Y;

        internal TrapData TrapData;


        internal override void Decode()
        {
            this.X = Reader.ReadInt32();
            this.Y = Reader.ReadInt32();

            this.TrapData = Reader.ReadData<TrapData>();

            base.Decode();
        }

        internal override void Execute()
        {
            if (this.TrapData != null)
            {
                var Level = Device.GameMode.Level;
                //if (!Level.IsBuildingCapReached(this.BuildingData))
                {
                    ResourceData ResourceData = this.TrapData.BuildResourceData;

                    if (ResourceData != null)
                    {
                        if (this.TrapData.TownHallLevel[0] <= (Level.GameObjectManager.Map == 0 ? Level.GameObjectManager.TownHall.GetUpgradeLevel() + 1 : Level.GameObjectManager.TownHall2.GetUpgradeLevel() + 1))
                        {
                            if (Level.Player.Resources.GetCountByData(ResourceData) >= this.TrapData.BuildCost[0])
                            {
                                Level.Player.Resources.Remove(ResourceData, this.TrapData.BuildCost[0]);
                                this.StartConstruction(Level);
                            }
                        }
                    }
                }
            }
        }

        internal void StartConstruction(Level Level)
        {
            Trap GameObject = new Trap(this.TrapData, Level);

            GameObject.SetUpgradeLevel(-1);

            GameObject.Position.X = this.X << 9;
            GameObject.Position.Y = this.Y << 9;

            GameObject.FinishConstruction(true);

            Level.GameObjectManager.AddGameObject(GameObject, Level.Player.Map);
        }
    }
}
