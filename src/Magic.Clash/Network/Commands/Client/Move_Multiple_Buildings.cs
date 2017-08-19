using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Move_Multiple_Buildings : Command
    {
        internal List<BuildingToMove> Buildings;
        internal int Tick;

        public Move_Multiple_Buildings(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            var buildingCount = Reader.ReadInt32();
            Buildings = new List<BuildingToMove>(buildingCount);
            for (var i = 0; i < buildingCount; ++i)
                Buildings.Add(new BuildingToMove
                {
                    XY = new[] {Reader.ReadInt32(), Reader.ReadInt32()},
                    Id = Reader.ReadInt32()
                });
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            foreach (var building in Buildings)
            {
                var go = Device.Player.GameObjectManager.GetGameObjectByID(building.Id,
                    Device.Player.Avatar.Variables.IsBuilderVillage);
                go?.SetPosition(building.XY[0], building.XY[1]);
            }
        }
    }
}
