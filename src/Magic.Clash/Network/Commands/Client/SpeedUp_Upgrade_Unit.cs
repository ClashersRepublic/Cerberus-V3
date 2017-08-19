using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SpeedUp_Upgrade_Unit : Command
    {
        internal int BuildingId;
        internal int Tick;

        public SpeedUp_Upgrade_Unit(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var Object =
                Device.Player.GameObjectManager.GetGameObjectByID(BuildingId,
                    Device.Player.Avatar.Variables.IsBuilderVillage);

            if (Object != null)
            {
                if (Object.ClassId == 0 || Object.ClassId == 7)
                {
                    var upgradeComponent = ((Construction_Item) Object).GetUnitUpgradeComponent();
                    if (upgradeComponent?.GetUnit != null)
                        upgradeComponent.SpeedUp();
                }
            }
            else
            {
                ExceptionLogger.Log(new NullReferenceException(),
                    $"Object with id {BuildingId} from user {Device.Player.Avatar.UserId} is null");
            }
        }
    }
}
