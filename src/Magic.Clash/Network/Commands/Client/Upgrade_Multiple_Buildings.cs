using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Structure;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Upgrade_Multiple_Buildings : Command
    {
        internal List<int> BuildingIds;
        internal bool IsAltResource;
        internal int Tick;

        public Upgrade_Multiple_Buildings(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            IsAltResource = Reader.ReadBoolean();
            var buildingCount = Reader.ReadInt32();
            BuildingIds = new List<int>(buildingCount);
            for (var i = 0; i < buildingCount; i++)
            {
                var buildingId = Reader.ReadInt32();
                BuildingIds.Add(buildingId);
            }
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var Avatar = Device.Player.Avatar;

            foreach (var buildingId in BuildingIds)
            {
                if (buildingId >= 500000000)
                {
                    var go = Device.Player.GameObjectManager.GetGameObjectByID(buildingId,
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
                                if (Avatar.HasEnoughResources(resource.GetGlobalId(),
                                    bd.GetBuildCost(b.GetUpgradeLevel)))
                                    if (Device.Player.Avatar.Variables.IsBuilderVillage
                                        ? Device.Player.HasFreeBuilderWorkers
                                        : Device.Player.HasFreeVillageWorkers)
                                    {
#if DEBUG
                                    var name = go.Data.Row.Name;
                                    if (b.ClassId == 0 || b.ClassId == 7)
                                        Logger.SayInfo(b.ClassId == 0
                                            ? $"Building: Upgrading {name} with ID {buildingId}"
                                            : $"Builder Building: Upgrading {name} with ID {buildingId}");
                                    else if (b.ClassId == 4 || b.ClassId == 11)
                                        Logger.SayInfo(b.ClassId == 4
                                            ? $"Trap: Upgrading {name} with ID {buildingId}"
                                            : $"Builder Trap: Upgrading {name} with ID {buildingId}");
                                    else if (b.ClassId == 8 || b.ClassId == 15)
                                        Logger.SayInfo(b.ClassId == 8
                                            ? $"Village Object: Upgrading {name} with ID {buildingId}"
                                            : $"Buildeer Village Object: Upgrading {name} with ID {buildingId}");
#endif
                                        if (bd.IsTownHall2())
                                        {
                                            if (Avatar.Builder_TownHall_Level == 0)
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

                                            Avatar.Builder_TownHall_Level++;
                                        }

                                        if (bd.IsAllianceCastle())
                                        {
                                            var a = (Building) go;
                                            var al = a.GetBuildingData;

                                            Avatar.Castle_Level++;
                                            Avatar.Castle_Total = al.GetUnitStorageCapacity(Avatar.Castle_Level);
                                            Avatar.Castle_Total_SP = al.GetAltUnitStorageCapacity(Avatar.Castle_Level);
                                        }
                                        else if (bd.IsTownHall())
                                        {
                                            Avatar.TownHall_Level++;
                                        }

                                        Avatar.Resources.Minus(resource.GetGlobalId(),
                                            bd.GetBuildCost(b.GetUpgradeLevel));
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
                else
                {
                    ShowValues();
                    new Out_Of_Sync(Device).Send();
                    break;
                }
            }
        }
    }
}
