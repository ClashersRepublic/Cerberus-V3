﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Debugs
{
    internal class Max_Base : Debug
    {
        internal int Level;
        public Max_Base(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
        }

        internal override Rank RequiredRank
        {
            get
            {
                return Rank.Player;              
            }
        }

        internal override void Process()
        {
            Player Player = this.Device.GameMode.Level.Player;

            GameObjectManager GameObjectManager = this.Device.GameMode.Level.GameObjectManager;
            if (this.Parameters.Length >= 1)
            {
                if (int.TryParse(this.Parameters[0], out this.Level))
                {
                    if (Player.TownHallLevel2 > 0)
                    {
                        if (!LevelFile.TownHallTemplate.TryGetValue(this.Level, out JObject Village))
                        {
                            if (File.Exists($"Gamefiles/level/townhall{this.Level}.json"))
                            {
                                Village = JObject.Parse(File.ReadAllText($"Gamefiles/level/townhall{this.Level}.json", Encoding.UTF8));
                                LevelFile.TownHallTemplate.Add(this.Level, Village);
                            }
                            else
                            {
                                this.SendChatMessage("There is no fucking village with that id nigger");
                                return;
                            }
                        }

                        for (int i = 0; i < GameObjectManager.GameObjects.Length; i++)
                        {
                            GameObjectManager.GameObjects[i][0].Clear();
                            GameObjectManager.GameObjects[i][1].Clear();
                        }

                        GameObjectManager.ObstaclesIndex[0] = 0;
                        GameObjectManager.ObstaclesIndex[1] = 0;

                        GameObjectManager.DecoIndex[0] = 0;
                        GameObjectManager.DecoIndex[1] = 0;

                        Player.Units2.Clear();

                        Player.Level.Load(Village);

                        Player.TownHallLevel = GameObjectManager.TownHall.GetUpgradeLevel();
                        Player.TownHallLevel2 = GameObjectManager.TownHall2.GetUpgradeLevel();
                        Player.CastleLevel = GameObjectManager.Bunker.GetUpgradeLevel();


                        Player.CastleTotalCapacity = GameObjectManager.Bunker.BuildingData.HousingSpace[Player.CastleLevel];
                        Player.CastleTotalSpellCapacity = GameObjectManager.Bunker.BuildingData.HousingSpaceAlt[Player.CastleLevel];

                        if (this.Level >= 4)
                        {
                            GameObjectManager.Boat.SetUpgradeLevel(GameObjectManager.Boat.VillageObjectData.MaxLevel);
                        }

                        foreach (var GameObject in GameObjectManager.Filter.GetGameObjects(0, 0))
                        {
                            var Building = (Building) GameObject;
                            if (Building.HeroBaseComponent != null)
                            {
                                if (CSV.Tables.Get(Gamefile.Heroes).GetData(Building.BuildingData.HeroType) is HeroData HeroData)
                                {
                                    if (Player.HeroUpgrades.GetByGlobalId(HeroData.GlobalId) == null)
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
                        }

                        foreach (var GameObject in GameObjectManager.Filter.GetGameObjects(0, 1))
                        {
                            var Building = (Building) GameObject;

                            if (Building.UnitStorageV2Component != null)
                            {
                                if (Building.UnitStorageV2Component.Units?.Count > 0)
                                {
                                    if (CSV.Tables.GetWithGlobalId(Building.UnitStorageV2Component.Units[0].Data) is CharacterData UnitData)
                                    {
                                        var UnitLevel = Player.GetUnitUpgradeLevel(UnitData);
                                        var UnitCount = UnitData.UnitsInCamp[UnitLevel];
                                        Player.Units2.Add(UnitData, UnitCount);
                                        Building.UnitStorageV2Component.Units[0].Count = UnitCount;
                                    }
                                }
                            }
                            else if (Building.HeroBaseComponent != null)
                            {
                                if (CSV.Tables.Get(Gamefile.Heroes).GetData(Building.BuildingData.HeroType) is HeroData HeroData)
                                {
                                    if (Player.HeroUpgrades.GetByGlobalId(HeroData.GlobalId) == null)
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
                            else if (Building.BuildingData.IsBarrack2)
                            {
                                Player.Variables.Village2BarrackLevel = Building.GetUpgradeLevel();
                            }
                        }

                        new OwnHomeDataMessage(this.Device).Send();
                    }
                    else
                    {
                        this.SendChatMessage("Please visit the builder village first before running this command!");
                    }
                }
                else
                {
                    this.SendChatMessage("You entered some shit!");
                }
            }
            else
            {
                this.SendChatMessage("Where is your parameters asshole!");
            }
        }
    }
}