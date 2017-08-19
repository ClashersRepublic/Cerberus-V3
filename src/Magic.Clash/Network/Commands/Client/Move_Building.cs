using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Move_Building : Command
    {
        internal int BuildingId;
        internal int Tick;
        internal int[] Position;

        public Move_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
        public override void Decode()
        {
            this.Position = new []{this.Reader.ReadInt32(), this.Reader.ReadInt32()};
            this.BuildingId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        public override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId, this.Device.Player.Avatar.Variables.IsBuilderVillage);
            go?.SetPosition(Position[0], Position[1]);
        }
    }
}