using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SpeedUp_Hero_Upgrade : Command
    {
        internal int BuildingId;
        internal uint Tick;

        public SpeedUp_Hero_Upgrade(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            Tick = Reader.ReadUInt32();
        }

        public override void Process()
        {
            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingId, Device.Player.Avatar.Variables.IsBuilderVillage);
            var hero = ((Construction_Item) go)?.GetHeroBaseComponent();
            hero?.SpeedUpUpgrade();
        }
    }
}
