using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Buy_Trap : Command
    {
        internal int[] XY;
        internal int TrapID;
        internal int Tick;

        public Buy_Trap(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            XY = new[] {Reader.ReadInt32(), Reader.ReadInt32()};
            TrapID = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;
            var td = (Traps) CSV.Tables.Get(Gamefile.Traps).GetDataWithID(TrapID);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Trap(td, Device.Player);

                if (ca.HasEnoughResources(td.GetBuildResource(0).GetGlobalId(), td.GetBuildCost(0)))
                    if (Device.Player.HasFreeVillageWorkers)
                    {
                        var rd = td.GetBuildResource(0);
                        ca.Resources.Minus(rd.GetGlobalId(), td.GetBuildCost(0));

                        b.StartConstructing(XY, false);
                        Device.Player.GameObjectManager.AddGameObject(b);
                    }
            }
            else
            {
                var b = new Builder_Trap(td, Device.Player);
                if (ca.HasEnoughResources(td.GetBuildResource(0).GetGlobalId(), td.GetBuildCost(0)))
                    if (Device.Player.HasFreeBuilderWorkers)
                    {
                        var rd = td.GetBuildResource(0);
                        ca.Resources.Minus(rd.GetGlobalId(), td.GetBuildCost(0));

                        b.StartConstructing(XY, true, true);
                        Device.Player.GameObjectManager.AddGameObject(b);
                    }
            }
        }
    }
}
