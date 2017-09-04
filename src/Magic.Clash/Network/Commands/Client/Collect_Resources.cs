using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Collect_Resources : Command
    {
        internal int BuildingID;
        internal int Tick;

        public Collect_Resources(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingID = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var Object =
                Device.Player.GameObjectManager.GetGameObjectByID(BuildingID,
                    Device.Player.Avatar.Variables.IsBuilderVillage);

            ((Construction_Item) Object)?.GetResourceProductionComponent()?.CollectResources();
        }
    }
}
