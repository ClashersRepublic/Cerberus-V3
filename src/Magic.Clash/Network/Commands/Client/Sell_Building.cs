using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Sell_Building : Command
    {
        internal int BuildingId;

        public Sell_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            Reader.ReadUInt32();
        }

        public override void Process()
        {
            var avatar = Device.Player.Avatar;
            var gameObjectById =
    Device.Player.GameObjectManager.GetGameObjectByID(BuildingId,
        Device.Player.Avatar.Variables.IsBuilderVillage);

            if (gameObjectById == null)
                return;
            if (gameObjectById.ClassId == 4 && gameObjectById.OppositeClassId == 11)
            {
                var trap = (Trap)gameObjectById;
                int upgradeLevel = trap.GetUpgradeLevel;
                var resource = trap.GetTrapData.GetBuildResource(upgradeLevel);
                int sellPrice = trap.GetTrapData.GetSellPrice(upgradeLevel);
                //  Obtain sell price remaining.
                Device.Player.GameObjectManager.RemoveGameObject((Game_Object)trap);
            }

            else
            {
                if (gameObjectById.ClassId != 6 && gameObjectById.OppositeClassId != 13)
                    return;
                var deco = (Deco)gameObjectById;
                var resource = deco.GetDecoData().GetBuildResource();
                int sellPrice = deco.GetDecoData().GetSellPrice();
                if (resource.PremiumCurrency)
                {
                }
                else
                {
                    Device.Player.GameObjectManager.RemoveGameObject((Game_Object)deco);
                }
            }
        }
    }
}