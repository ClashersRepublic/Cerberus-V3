using System;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Upgrade_Building : Command
    {
        internal int BuildingId;
        internal uint Unknown1;
        internal bool IsAltResource;

        public Upgrade_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingId = Reader.ReadInt32();
            IsAltResource = Reader.ReadBoolean();
            Unknown1 = Reader.ReadUInt32();
        }

        public override void Process()
        {
            var ca = Device.Player.Avatar;
            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingId,
                Device.Player.Avatar.Variables.IsBuilderVillage);
            if (go != null)
            {
                var b = (Construction_Item) go;
                if (b.CanUpgrade)
                {
                    var bd = b.GetConstructionItemData;
                    var resource = b.ClassId == 0 || b.ClassId == 7
                        ? IsAltResource
                            ? (bd as Buildings)?.GetAltBuildResource(b.GetUpgradeLevel + 1)
                            : bd.GetBuildResource(b.GetUpgradeLevel + 1)
                        : bd.GetBuildResource(b.GetUpgradeLevel + 1);

                    if (resource != null)
                    {
                        if (ca.HasEnoughResources(resource.GetGlobalId(), bd.GetBuildCost(b.GetUpgradeLevel)))
                            if (Device.Player.Avatar.Variables.IsBuilderVillage
                                ? Device.Player.HasFreeBuilderWorkers
                                : Device.Player.HasFreeVillageWorkers)
                            {
#if DEBUG
                                var name = go.Data.Row.Name;
                                if (b.ClassId == 0 || b.ClassId == 7)
                                    Logger.SayInfo(b.ClassId == 0
                                        ? $"Building: Upgrading {name} with ID {BuildingId}"
                                        : $"Builder Building: Upgrading {name} with ID {BuildingId}");
                                else if (b.ClassId == 4 || b.ClassId == 11)
                                    Logger.SayInfo(b.ClassId == 4
                                        ? $"Trap: Upgrading {name} with ID {BuildingId}"
                                        : $"Builder Trap: Upgrading {name} with ID {BuildingId}");
                                else if (b.ClassId == 8 || b.ClassId == 15)
                                    Logger.SayInfo(b.ClassId == 8
                                        ? $"Village Object: Upgrading {name} with ID {BuildingId}"
                                        : $"Buildeer Village Object: Upgrading {name} with ID {BuildingId}");
#endif

                                if (bd.IsTownHall2())
                                {
                                    if (ca.Builder_TownHall_Level == 0)
                                        Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(7),
                                            Object =>
                                            {
                                                var b2 = (Builder_Building) Object;
                                                var bd2 = b2.GetBuildingData;
                                                if (b2.Locked)
                                                {
                                                    if (bd2.Locked)
                                                        return;
#if DEBUG
                                                    Logger.SayInfo(
                                                        $"Builder Building: Unlocking {bd2.Name} with ID {Object.GlobalId}");
#endif
                                                    b2.Locked = false;
                                                }
                                            });

                                    ca.Builder_TownHall_Level++;
                                }

                                if (bd.IsAllianceCastle())
                                {
                                    var a = (Building) go;
                                    var al = a.GetBuildingData;

                                    ca.Castle_Level++;
                                    ca.Castle_Total = al.GetUnitStorageCapacity(ca.Castle_Level);
                                    ca.Castle_Total_SP = al.GetAltUnitStorageCapacity(ca.Castle_Level);
                                }
                                else if (bd.IsTownHall())
                                {
                                    ca.TownHall_Level++;
                                }

                                ca.Resources.Minus(resource.GetGlobalId(), bd.GetBuildCost(b.GetUpgradeLevel));
                                b.StartUpgrading(Device.Player.Avatar.Variables.IsBuilderVillage);
                            }
                    }
                    else
                    {
                        ExceptionLogger.Log(new NullReferenceException(),
                            $"Resource data is null for building with {b.ClassId} class and {b.GlobalId} global id");
                    }
                }
            }
        }
    }
}
