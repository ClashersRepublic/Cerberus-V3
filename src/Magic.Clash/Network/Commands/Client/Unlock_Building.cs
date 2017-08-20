using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Unlock_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;

        public Unlock_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            Unknown1 = Reader.ReadUInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;

            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingId,
                Device.Player.Avatar.Variables.IsBuilderVillage);

            var b = (Construction_Item) go;

            var bd = (Buildings) b.GetConstructionItemData;

            if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel).GetGlobalId(),
                bd.GetBuildCost(b.GetUpgradeLevel)))
            {
#if DEBUG
                var name = go.Data.Row.Name;

                Logger.SayInfo(b.ClassId == 0
                    ? "Building" + $" : Upgrading {name} Unlocking ID {BuildingId}"
                    : "Builder Building" + $" : Unlocking {name} with ID {BuildingId}");
#endif
                if (bd.IsAllianceCastle())
                {
                    var a = (Building) go;
                    var al = a.GetBuildingData;

                    ca.Castle_Level++;
                    ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level);
                    ca.Castle_Total_SP = al.GetAltUnitStorageCapacity(ca.Castle_Level);
                }
                if (bd.IsHeroBarrack)
                {
                    if (b.GetHeroBaseComponent(true) != null)
                    {
                        var data = (Buildings) b.Data;
                        var hd = CSV.Tables.Get(Gamefile.Heroes).GetData(data.HeroType) as Heroes;
                        Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                        Device.Player.Avatar.SetHeroHealth(hd, 0);
                        Device.Player.Avatar.SetHeroState(hd, 3);
                    }
                }
                var rd = bd.GetBuildResource(b.GetUpgradeLevel);
                ca.Resources.Minus(rd.GetGlobalId(), bd.GetBuildCost(b.GetUpgradeLevel));
                b.Locked = false;
            }
        }
    }
}