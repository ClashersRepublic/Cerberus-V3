using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Buy_Multiple_Wall : Command
    {
        internal List<int[]> WallXYs;
        internal int WallID;
        internal int Count;
        internal int Tick;

        public Buy_Multiple_Wall(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            WallXYs = new List<int[]>(Count = Reader.ReadInt32());

            for (var i = 0; i < Count; i++)
                WallXYs.Add(new[] {Reader.ReadInt32(), Reader.ReadInt32()});
            WallID = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            Device.Player.Avatar.Wall_Group_ID++;
            foreach (var WallXY in WallXYs)
            {
                var bd = (Buildings) CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(WallID);
                if (!Device.Player.Avatar.Variables.IsBuilderVillage)
                {
                    var b = new Building(bd, Device.Player);

                    if (Device.Player.Avatar.HasEnoughResources(bd.GetBuildResource(0).GetGlobalId(),
                        bd.GetBuildCost(0)))
                        if (Device.Player.HasFreeVillageWorkers)
                        {
                            var rd = bd.GetBuildResource(0);
                            Device.Player.Avatar.Resources.Minus(rd.GetGlobalId(), bd.GetBuildCost(0));

                            var a = (Combat_Component) b.GetComponent(1, false);
                            a.WallI = Device.Player.Avatar.Wall_Group_ID;
                            b.StartConstructing(WallXY, Device.Player.Avatar.Variables.IsBuilderVillage);
                            Device.Player.GameObjectManager.AddGameObject(b);
                        }
                }
                else
                {
                    var b = new Builder_Building(bd, Device.Player);
                    if (Device.Player.Avatar.HasEnoughResources(bd.GetBuildResource(0).GetGlobalId(),
                        bd.GetBuildCost(0)))
                        if (Device.Player.HasFreeBuilderWorkers)
                        {
                            var rd = bd.GetBuildResource(0);
                            Device.Player.Avatar.Resources.Minus(rd.GetGlobalId(), bd.GetBuildCost(0));
                            var a = (Combat_Component) b.GetComponent(1, false);
                            a.WallI = Device.Player.Avatar.Wall_Group_ID;
                            b.StartConstructing(WallXY, Device.Player.Avatar.Variables.IsBuilderVillage);
                            Device.Player.GameObjectManager.AddGameObject(b);
                        }
                }
            }
        }
    }
}
