using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Debugs
{
    internal class Max_Village : Debug
    {
        internal int VillageID;
        internal StringBuilder Help;

        public Max_Village(Device device, params string[] Parameters) : base(device, Parameters)
        {
        }

        internal override void Process()
        {
            if (Parameters.Length >= 1)
            {
                if (int.TryParse(Parameters[0], out VillageID))
                {
                    switch (VillageID)
                    {
                        case 0:

                            Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(0), b =>
                            {
                                var building = (Construction_Item) b;
                                var data = (Buildings) building.GetConstructionItemData;

                                if (building.Locked)
                                    building.Locked = false;

                                if (building.IsConstructing)
                                    building.IsConstructing = false;

                                if (data.IsTownHall())
                                    Device.Player.Avatar.TownHall_Level = data.GetUpgradeLevelCount() - 1;

                                if (data.IsAllianceCastle())
                                {
                                    var al = ((Building) b).GetBuildingData;
                                    Device.Player.Avatar.Castle_Level = data.GetUpgradeLevelCount() - 1;
                                    Device.Player.Avatar.Castle_Total =
                                        al.GetUnitStorageCapacity(Device.Player.Avatar.Castle_Level);
                                    Device.Player.Avatar.Castle_Total_SP =
                                        al.GetAltUnitStorageCapacity(Device.Player.Avatar.Castle_Level);
                                }

                                building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                            });

                            Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(4), t =>
                            {
                                var trap = (Trap) t;
                                var data = (Traps) trap.Data;

                                trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                            });

                            SendChatMessage("Command Processor: Success, Enjoy!");
                            new Own_Home_Data(Device).Send();

                            break;

                        case 1:
                            if (Device.Player.Avatar.Builder_TownHall_Level > 0)
                            {
                                Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(7), b =>
                                {
                                    var building = (Construction_Item) b;
                                    var data = (Buildings) building.GetConstructionItemData;

                                    if (building.Locked)
                                    {
                                        building.Locked = false;
                                        if (data.IsHeroBarrack)
                                            if (building.GetHeroBaseComponent(true) != null)
                                            {
                                                var hd = CSV.Tables.Get(Gamefile.Heroes)
                                                    .GetData(data.HeroType) as Heroes;
                                                Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                                                Device.Player.Avatar.SetHeroHealth(hd, 0);
                                                Device.Player.Avatar.SetHeroState(hd, 3);
                                            }
                                    }

                                    if (building.IsConstructing)
                                        building.IsConstructing = false;

                                    if (data.IsTownHall2())
                                        Device.Player.Avatar.Builder_TownHall_Level =
                                            data.GetUpgradeLevelCount() - 1;
                                    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(11), t =>
                                {
                                    var trap = (Builder_Trap) t;
                                    var data = (Traps) trap.Data;

                                    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                SendChatMessage("Command Processor: Success, Enjoy!");
                                new Own_Home_Data(Device).Send();
                            }
                            else
                            {
                                SendChatMessage(
                                    "Command Processor: Please visit builder village first before running this mode!");
                            }
                            break;

                        case 2:
                            if (Device.Player.Avatar.Builder_TownHall_Level > 0)
                            {
                                Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(0), b =>
                                {
                                    var building = (Building) b;
                                    var data = (Buildings) building.Data;

                                    if (building.Locked)
                                        building.Locked = false;

                                    if (building.IsConstructing)
                                        building.IsConstructing = false;

                                    if (data.IsTownHall())
                                        Device.Player.Avatar.TownHall_Level = data.GetUpgradeLevelCount() - 1;

                                    if (data.IsAllianceCastle())
                                    {
                                        var al = ((Building) b).GetBuildingData;
                                        Device.Player.Avatar.Castle_Level = data.GetUpgradeLevelCount() - 1;
                                        Device.Player.Avatar.Castle_Total =
                                            al.GetUnitStorageCapacity(Device.Player.Avatar.Castle_Level);
                                        Device.Player.Avatar.Castle_Total_SP =
                                            al.GetAltUnitStorageCapacity(Device.Player.Avatar.Castle_Level);
                                    }


                                    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(4), t =>
                                {
                                    var trap = (Trap) t;
                                    var data = (Traps) trap.Data;

                                    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(7), b =>
                                {
                                    var building = (Construction_Item) b;
                                    var data = (Buildings) building.GetConstructionItemData;


                                    if (building.Locked)
                                    {
                                        building.Locked = false;
                                        if (data.IsHeroBarrack)
                                            if (building.GetHeroBaseComponent(true) != null)
                                            {
                                                var hd = CSV.Tables.Get(Gamefile.Heroes)
                                                    .GetData(data.HeroType) as Heroes;
                                                Device.Player.Avatar.SetUnitUpgradeLevel(hd, 0);
                                                Device.Player.Avatar.SetHeroHealth(hd, 0);
                                                Device.Player.Avatar.SetHeroState(hd, 3);
                                            }
                                    }

                                    if (building.IsConstructing)
                                        building.IsConstructing = false;

                                    if (data.IsTownHall2())
                                        Device.Player.Avatar.Builder_TownHall_Level =
                                            data.GetUpgradeLevelCount() - 1;

                                    building.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                Parallel.ForEach(Device.Player.GameObjectManager.GetGameObjects(11), t =>
                                {
                                    var trap = (Builder_Trap) t;
                                    var data = (Traps) trap.Data;

                                    trap.SetUpgradeLevel(data.GetUpgradeLevelCount() - 1);
                                });

                                SendChatMessage("Command Processor: Success, Enjoy!");
                                new Own_Home_Data(Device).Send();
                            }
                            else
                            {
                                SendChatMessage(
                                    "Command Processor: Please visit builder village first before running this mode!");
                            }
                            break;

                        default:
                            Help = new StringBuilder();
                            Help.AppendLine("Available village types:");
                            Help.AppendLine("\t 0 = Normal Village");
                            Help.AppendLine("\t 1 = Builder Village (Make sure you have unlock builder base first!)");
                            Help.AppendLine("\t 2 = All Village (Make sure you have unlock builder base first!)");
                            Help.AppendLine();
                            Help.AppendLine("Command:");
                            Help.AppendLine("\t/max_village {village-id}");
                            SendChatMessage(Help.ToString());
                            Help = null;
                            return;
                    }
                }
                else
                {
                    Help = new StringBuilder();
                    Help.AppendLine("Available village types:");
                    Help.AppendLine("\t 0 = Normal Village");
                    Help.AppendLine("\t 1 = Builder Village (Make sure you have unlock builder base first!)");
                    Help.AppendLine("\t 2 = All Village (Make sure you have unlock builder base first!)");
                    Help.AppendLine();
                    Help.AppendLine("Command:");
                    Help.AppendLine("\t/max_village {village-id}");
                    SendChatMessage(Help.ToString());
                    Help = null;
                }
            }
            else
            {
                Help = new StringBuilder();
                Help.AppendLine("Available village types:");
                Help.AppendLine("\t 0 = Normal Village");
                Help.AppendLine("\t 1 = Builder Village (Make sure you have unlock builder base first!)");
                Help.AppendLine("\t 2 = All Village (Make sure you have unlock builder base first!)");
                Help.AppendLine();
                Help.AppendLine("Command:");
                Help.AppendLine("\t/max_village {village-id}");
                SendChatMessage(Help.ToString());
                Help = null;
            }
        }
    }
}