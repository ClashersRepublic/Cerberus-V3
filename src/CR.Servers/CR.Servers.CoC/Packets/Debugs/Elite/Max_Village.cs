namespace CR.Servers.CoC.Packets.Debugs
{
    using System.Text;
    using System.Threading.Tasks;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Logic.Enums;

    internal class Max_Village : Debug
    {
        internal StringBuilder Help;

        internal int VillageID;

        public Max_Village(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
        }

        internal override Rank RequiredRank
        {
            get
            {
                return Rank.Elite;
            }
        }

        internal override void Process()
        {
            if (this.Parameters.Length >= 1)
            {
                if (int.TryParse(this.Parameters[0], out this.VillageID))
                {
                    Player Player = this.Device.GameMode.Level.Player;
                    switch (this.VillageID)
                    {
                        case 0:
                        {
                            Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(0, 0), GameObject =>
                            {
                                Building Building = (Building) GameObject;
                                BuildingData Data = Building.BuildingData;

                                if (Building.Locked)
                                {
                                    Building.Locked = false;
                                }

                                if (Building.Constructing)
                                {
                                    Building.ConstructionTimer = null;
                                }

                                if (Data.IsTownHall)
                                {
                                    Player.TownHallLevel = Data.MaxLevel;
                                }

                                if (Data.IsAllianceCastle)
                                {
                                    Player.CastleLevel = Data.MaxLevel;
                                    Player.CastleTotalCapacity = Data.HousingSpace[Player.CastleLevel];
                                    Player.CastleTotalSpellCapacity = Data.HousingSpaceAlt[Player.CastleLevel];
                                }

                                Building.SetUpgradeLevel(Data.MaxLevel);
                            });

                            Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(4, 0), GameObject =>
                            {
                                Trap Trap = (Trap) GameObject;
                                TrapData Data = (TrapData) Trap.Data;

                                if (Trap.Constructing)
                                {
                                    Trap.ConstructionTimer = null;
                                }

                                Trap.SetUpgradeLevel(Data.MaxLevel);
                            });

                            this.SendChatMessage("Successfully maxed your normal village, Enjoy!");

                            new OwnHomeDataMessage(this.Device).Send();

                            break;
                        }

                        case 1:
                        {
                            if (Player.TownHallLevel2 > 0)
                            {
                                Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(0, 1), GameObject =>
                                {
                                    Building Building = (Building) GameObject;
                                    BuildingData Data = Building.BuildingData;

                                    if (Building.Locked)
                                    {
                                        if (Data.IsHeroBarrack)
                                        {
                                            if (Building.HeroBaseComponent != null)
                                            {
                                                if (CSV.Tables.Get(Gamefile.Heroes).GetData(Data.HeroType) is HeroData HeroData)
                                                {
                                                    Player.HeroUpgrades.Set(HeroData, 0);
                                                    Player.HeroStates.Set(HeroData, 3);

                                                    if (HeroData.HasAltMode)
                                                    {
                                                        Player.HeroModes.Set(HeroData, 0);
                                                    }
                                                }
                                            }
                                        }

                                        Building.Locked = false;
                                    }

                                    if (Building.Constructing)
                                    {
                                        Building.ConstructionTimer = null;
                                    }

                                    if (Data.IsTownHall2)
                                    {
                                        Player.TownHallLevel2 = Data.MaxLevel;
                                    }

                                    if (Data.IsBarrack2)
                                    {
                                        Player.Variables.Village2BarrackLevel = Data.MaxLevel;
                                    }

                                    Building.SetUpgradeLevel(Data.MaxLevel);
                                });

                                Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(4, 1), GameObject =>
                                {
                                    Trap Trap = (Trap) GameObject;
                                    TrapData Data = (TrapData) Trap.Data;

                                    if (Trap.Constructing)
                                    {
                                        Trap.ConstructionTimer = null;
                                    }

                                    Trap.SetUpgradeLevel(Data.MaxLevel);
                                });

                                this.SendChatMessage("Successfully maxed your builder village, Enjoy!");

                                new OwnHomeDataMessage(this.Device).Send();
                            }
                            else
                            {
                                this.SendChatMessage("Please visit the builder village first before running this mode!");
                            }

                            break;
                        }


                        case 2:
                        {
                            if (Player.TownHallLevel2 > 0)
                            {
                                Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(0, 0), GameObject =>
                                {
                                    Building Building = (Building) GameObject;
                                    BuildingData Data = Building.BuildingData;

                                    if (Building.Locked)
                                    {
                                        Building.Locked = false;
                                    }

                                    if (Building.Constructing)
                                    {
                                        Building.ConstructionTimer = null;
                                    }

                                    if (Data.IsTownHall)
                                    {
                                        Player.TownHallLevel = Data.MaxLevel;
                                    }

                                    if (Data.IsAllianceCastle)
                                    {
                                        Player.CastleLevel = Data.MaxLevel;
                                        Player.CastleTotalCapacity = Data.HousingSpace[Player.CastleLevel];
                                        Player.CastleTotalSpellCapacity = Data.HousingSpaceAlt[Player.CastleLevel];
                                    }

                                    Building.SetUpgradeLevel(Data.MaxLevel);
                                });

                                Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(0, 1), GameObject =>
                                {
                                    Building Building = (Building) GameObject;
                                    BuildingData Data = Building.BuildingData;

                                    if (Building.Locked)
                                    {
                                        if (Data.IsHeroBarrack)
                                        {
                                            if (Building.HeroBaseComponent != null)
                                            {
                                                if (CSV.Tables.Get(Gamefile.Heroes).GetData(Data.HeroType) is HeroData HeroData)
                                                {
                                                    Player.HeroUpgrades.Set(HeroData, 0);
                                                    Player.HeroStates.Set(HeroData, 3);

                                                    if (HeroData.HasAltMode)
                                                    {
                                                        Player.HeroModes.Set(HeroData, 0);
                                                    }
                                                }
                                            }
                                        }

                                        Building.Locked = false;
                                    }

                                    if (Building.Constructing)
                                    {
                                        Building.ConstructionTimer = null;
                                    }

                                    if (Data.IsTownHall2)
                                    {
                                        Player.TownHallLevel2 = Data.MaxLevel;
                                    }

                                    if (Data.IsBarrack2)
                                    {
                                        Player.Variables.Village2BarrackLevel = Data.MaxLevel;
                                    }

                                    Building.SetUpgradeLevel(Data.MaxLevel);
                                });

                                Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(4, 0), GameObject =>
                                {
                                    Trap Trap = (Trap) GameObject;
                                    TrapData Data = (TrapData) Trap.Data;

                                    if (Trap.Constructing)
                                    {
                                        Trap.ConstructionTimer = null;
                                    }

                                    Trap.SetUpgradeLevel(Data.MaxLevel);
                                });

                                Parallel.ForEach(this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjects(4, 1), GameObject =>
                                {
                                    Trap Trap = (Trap) GameObject;
                                    TrapData Data = (TrapData) Trap.Data;

                                    if (Trap.Constructing)
                                    {
                                        Trap.ConstructionTimer = null;
                                    }

                                    Trap.SetUpgradeLevel(Data.MaxLevel);
                                });

                                this.SendChatMessage("Successfully maxed your both of village, Enjoy!");

                                new OwnHomeDataMessage(this.Device).Send();
                            }
                            else
                            {
                                this.SendChatMessage("Please visit the builder village first before running this mode!");
                            }

                            break;
                        }

                        default:
                            this.Help = new StringBuilder();
                            this.Help.AppendLine("Available village types:\n\t0 = Normal Village\n\t1 = Builder Village (Make sure you have unlocked the builder base first!)\n\t2 = All Village (Make sure you have unlocked the builder base first!)");
                            this.Help.AppendLine("Command:\n\t/maxvillage {village-id");
                            this.SendChatMessage(this.Help.ToString());
                            this.Help = null;
                            break;
                    }
                }
                else
                {
                    this.Help = new StringBuilder();
                    this.Help.AppendLine("Available village types:\n\t0 = Normal Village\n\t1 = Builder Village (Make sure you have unlocked the builder base first!)\n\t2 = All Village (Make sure you have unlocked the builder base first!)");
                    this.Help.AppendLine("Command:\n\t/maxvillage {village-id");
                    this.SendChatMessage(this.Help.ToString());
                    this.Help = null;
                }
            }
            else
            {
                this.Help = new StringBuilder();
                this.Help.AppendLine("Available village types:\n\t0 = Normal Village\n\t1 = Builder Village (Make sure you have unlocked the builder base first!)\n\t2 = All Village (Make sure you have unlocked the builder base first!)");
                this.Help.AppendLine("Command:\n\t/maxvillage {village-id");
                this.SendChatMessage(this.Help.ToString());
                this.Help = null;
            }
        }
    }
}