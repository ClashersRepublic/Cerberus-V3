using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Sell_Building : Command
    {
        internal override int Type => 503;

        public Sell_Building(Device Device, Reader Reader) : base (Device, Reader)
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
            var Building = Level.GameObjectManager.Filter.GetGameObjectByPreciseId(this.BuildingId);

            if (Building != null)
            {
                if (Building is Deco Deco)
                {
                    DecoData Data = Deco.DecoData;
                    ResourceData ResourceData = Data.BuildResourceData;

                    if (ResourceData != null)
                    {
                        int SellPrice = Data.SellPrice;
                        if (ResourceData.PremiumCurrency)
                        {
                            Level.Player.AddDiamonds(SellPrice);
                        }
                        else if (Level.Player.Resources.GetCountByData(ResourceData) >= SellPrice)
                        {
                            Level.Player.Resources.Plus(ResourceData.GlobalId, SellPrice);
                            Level.GameObjectManager.RemoveGameObject(Deco, Data.VillageType);
                        }
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to sell buidling. The resource data is null!");
                }
                else
                    Logging.Error(this.GetType(), "Unable to sell buidling. The player tried to sell unsurported buidling!");
            }
            else
                Logging.Error(this.GetType(), "Unable to sell buidling. The player tried to sell a null building!");
        }
    }
}
