using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SpeedUp_Construction : Command
    {
        internal int BuildingId;

        public SpeedUp_Construction(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            Reader.ReadInt32();
        }

        public override void Process()
        {
            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingId,
                Device.Player.Avatar.Variables.IsBuilderVillage);

            if (go != null)
                if (go.ClassId == 0 || go.ClassId == 4 || go.ClassId == 7 || go.ClassId == 11)
                    ((Construction_Item) go).SpeedUpConstruction();
        }
    }
}
