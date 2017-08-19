using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Buy_Building : Command
    {
        internal int BuildingId;
        internal int Tick;
        internal int[] XY = new int[2];

        public Buy_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            XY[0] = Reader.ReadInt32();
            XY[1] = Reader.ReadInt32();
            BuildingId = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;
            var bd = (Buildings) CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(BuildingId);
            if (!ca.Variables.IsBuilderVillage)
            {
                var b = new Building(bd, Device.Player);

                if (ca.HasEnoughResources(bd.GetBuildResource(0).GetGlobalId(), bd.GetBuildCost(0)))
                {
                    if (bd.IsWorkerBuilding())
                    {
                        if (Device.Player.VillageWorkerManager.GetFreeWorkers() > 0)
                        {
                            var Cost = 0;
                            var row = CSV.Tables.Get(Gamefile.Globals);
                            if (Device.Player.VillageWorkerManager.GetTotalWorkers() == 1)
                                Cost = ((Globals) row.GetData("WORKER_COST_2ND")).NumberValue;
                            else if (Device.Player.VillageWorkerManager.GetTotalWorkers() == 2)
                                Cost = ((Globals) row.GetData("WORKER_COST_3RD")).NumberValue;
                            else if (Device.Player.VillageWorkerManager.GetTotalWorkers() == 3)
                                Cost = ((Globals) row.GetData("WORKER_COST_4TH")).NumberValue;
                            else if (Device.Player.VillageWorkerManager.GetTotalWorkers() >= 4)
                                Cost = ((Globals) row.GetData("WORKER_COST_5TH")).NumberValue;

                            var rd = bd.GetBuildResource(0);
                            ca.Resources.Minus(rd.GetGlobalId(), Cost);
                        }
                        b.StartConstructing(XY, false);
                        Device.Player.GameObjectManager.AddGameObject(b);
                        return;
                    }

                    if (Device.Player.HasFreeVillageWorkers)
                    {
                        var rd = bd.GetBuildResource(0);
                        ca.Resources.Minus(rd.GetGlobalId(), bd.GetBuildCost(0));

                        b.StartConstructing(XY, false);
                        Device.Player.GameObjectManager.AddGameObject(b);
                    }
                }
            }
            else
            {
                var b = new Builder_Building(bd, Device.Player);
                if (ca.HasEnoughResources(bd.GetBuildResource(0).GetGlobalId(), bd.GetBuildCost(0)))
                {
                    if (bd.IsWorker2Building())
                    {
                        b.StartConstructing(XY, true);
                        Device.Player.GameObjectManager.AddGameObject(b);
                        return;
                    }

                    if (Device.Player.HasFreeBuilderWorkers)
                    {
                        var rd = bd.GetBuildResource(0);
                        ca.Resources.Minus(rd.GetGlobalId(), bd.GetBuildCost(0));

                        b.StartConstructing(XY, true);
                        Device.Player.GameObjectManager.AddGameObject(b);
                    }
                }
            }
        }
    }
}
