using System;
using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;

namespace CR.Servers.CoC.Logic
{
    internal class ComponentManager
    {
        internal Level Level;
        internal List<Component>[] Components;

        public ComponentManager(Level Level)
        {
            this.Level = Level;
            this.Components = new List<Component>[20];

            for (int i = 0; i < 20; i++)
            {
                this.Components[i] = new List<Component>(32);
            }
        }

        internal void AddComponent(Component Component)
        {
            this.Components[Component.Parent.Type].Add(Component);
        }

        internal void RemoveComponent(Component Component)
        {
            this.Components[Component.Parent.Type].Remove(Component);
        }

        internal void FindAll(Predicate<Component> Match)
        {
            List<Component> Components = new List<Component>(16);

            this.Components[0].ForEach(Component =>
            {
                if (Match(Component))
                {
                    Components.Add(Component);
                }
            });

            this.Components[1].ForEach(Component =>
            {
                if (Match(Component))
                {
                    Components.Add(Component);
                }
            });
        }


        internal List<Building> TroopHousings
        {
            get
            {
                List<Building> TroopHousings = new List<Building>(8);

                this.Level.GameObjectManager.GameObjects[0][0].ForEach(GameObject =>
                {
                    Building Building = (Building)GameObject;

                    if (Building.BuildingData.IsTrainingHousing)
                    {
                        if (Building.GetUpgradeLevel() > -1)
                        {
                            TroopHousings.Add(Building);
                        }
                    }
                });

                return TroopHousings;
            }
        }

        internal List<Building> DarkSpellForge
        {
            get
            {
                List<Building> DarkSpellForge = new List<Building>();

                this.Level.GameObjectManager.GameObjects[0][0].ForEach(GameObject =>
                {
                    Building Building = (Building)GameObject;

                    if (Building.BuildingData.IsSpellForge && Building.BuildingData.IsMiniSpellForge)
                    {
                        if (Building.GetUpgradeLevel() > -1)
                        {
                            DarkSpellForge.Add(Building);
                        }
                    }
                });

                return DarkSpellForge;
            }
        }


        internal List<Building> DarkBarracks
        {
            get
            {
                List<Building> DarkBarracks = new List<Building>();

                this.Level.GameObjectManager.GameObjects[0][0].ForEach(GameObject =>
                {
                    Building Building = (Building)GameObject;

                    if (Building.BuildingData.IsDarkBarrack)
                    {
                        if (Building.GetUpgradeLevel() > -1)
                        {
                            DarkBarracks.Add(Building);
                        }
                    }
                });

                return DarkBarracks;
            }
        }

        internal List<Building> SpellForge
        {
            get
            {
                List<Building> SpellForge = new List<Building>();

                this.Level.GameObjectManager.GameObjects[0][0].ForEach(GameObject =>
                {
                    Building Building = (Building)GameObject;

                    if (Building.BuildingData.IsSpellForge && !Building.BuildingData.IsMiniSpellForge)
                    {
                        if (Building.GetUpgradeLevel() > -1)
                        {
                            SpellForge.Add(Building);
                        }
                    }
                });

                return SpellForge;
            }
        }

        internal List<Building> Barracks
        {
            get
            {
                List<Building> Barracks = new List<Building>();

                this.Level.GameObjectManager.GameObjects[0][0].ForEach(GameObject =>
                {
                    Building Building = (Building)GameObject;

                    if (Building.BuildingData.IsBarrack)
                    {
                        if (Building.GetUpgradeLevel() > -1)
                        {
                            Barracks.Add(Building);
                        }
                    }
                });

                return Barracks;
            }
        }

        internal List<Component> FindAll(int GameObjectType, Predicate<Component> Match)
        {
            List<Component> Components = new List<Component>(16);

            this.Components[GameObjectType].ForEach(Component =>
            {
                if (Match(Component))
                {
                    Components.Add(Component);
                }
            });

            return Components;
        }

        internal void RefreshResourceCaps()
        {
            if (this.Level.Player != null)
            {
                Player Player = this.Level.Player;

                Player.ResourceCaps.ForEach(Slot =>
                {
                    Slot.Count = 0;
                });

                this.Components[0].ForEach(Component =>
                {
                    if (Component.Type == 6)
                    {
                        ResourceStorageComponent StorageComponent = (ResourceStorageComponent)Component;

                        foreach (ResourceData Data in CSV.Tables.Get(Gamefile.Resources).Datas)
                        {
                            Player.ResourceCaps.Add(Data, StorageComponent.GetMax(Data.GetId()));
                        }
                    }
                });
            }
             else
                Logging.Info(this.GetType(), "Unable to refresh resource caps. Player is NULL.");
        }

        internal static Component GetClosestComponent(int X, int Y, List<Component> Components)
        {
            Component Closest = null;
            int ClosestDistance = 0;

            Components.ForEach(Component =>
            {
                if (Closest == null || ClosestDistance > Component.Parent.Position.GetDistanceSquaredHelper(X, Y))
                {
                    Closest = Component;
                }
            });

            return Closest;
        }

        internal int TotalMaxSpellHousing
        {
            get
            {
                int Count = 0;

                this.Components[0].ForEach(Component =>
                {
                    if (Component.Type == 0)
                    {
                        var Storage = (UnitStorageComponent)Component;
                        if (Storage.IsSpellForge)
                        {
                            Count += ((UnitStorageComponent)Component).HousingSpace;
                        }
                    }
                });

                return Count;
            }
        }


        internal int TotalMaxHousing
        {
            get
            {
                int Count = 0;

                this.Components[0].ForEach(Component =>
                {
                    if (Component.Type == 0)
                    {
                        var Storage = (UnitStorageComponent) Component;
                        if (!Storage.IsSpellForge)
                        {
                            Count += ((UnitStorageComponent) Component).HousingSpace;
                        }
                    }
                });

                return Count;
            }
        }


        
        internal int MaxBarrackV2Level
        {
            get
            {
                int MaxLevel = -1;

                this.Components[0].ForEach(Component =>
                {
                    Building Building = (Building)Component.Parent;

                    if (Building.BuildingData.VillageType == 1)
                    {
                        if (Building.BuildingData.IsBarrack)
                        {
                            MaxLevel = Math.Max(MaxLevel, Building.GetUpgradeLevel());
                        }
                    }
                });

                return MaxLevel;
            }
        }

        internal int MaxBarrackLevel
        {
            get
            {
                int MaxLevel = -1;

                this.Components[0].ForEach(Component =>
                {
                    Building Building = (Building)Component.Parent;

                    if (Building.BuildingData.IsBarrack)
                    {
                        MaxLevel = Math.Max(MaxLevel, Building.GetUpgradeLevel());
                    }
                });

                return MaxLevel;
            }
        }

        internal int MaxSpellForgeLevel
        {
            get
            {
                int MaxLevel = -1;

                this.Components[0].ForEach(Component =>
                {
                    Building Building = (Building)Component.Parent;

                    if (Building.BuildingData.IsSpellForge && !Building.BuildingData.IsMiniSpellForge)
                    {
                        MaxLevel = Math.Max(MaxLevel, Building.GetUpgradeLevel());
                    }
                });

                return MaxLevel;
            }
        }

        internal int MaxDarkBarrackLevel
        {
            get
            {
                int MaxLevel = -1;

                this.Components[0].ForEach(Component =>
                {
                    Building Building = (Building)Component.Parent;

                    if (Building.BuildingData.IsDarkBarrack)
                    {
                        MaxLevel = Math.Max(MaxLevel, Building.GetUpgradeLevel());
                    }
                });

                return MaxLevel;
            }
        }

        internal int MaxDarkSpellForgeLevel
        {
            get
            {
                int MaxLevel = -1;

                this.Components[0].ForEach(Component =>
                {
                    Building Building = (Building)Component.Parent;

                    if (Building.BuildingData.IsSpellForge && Building.BuildingData.IsMiniSpellForge)
                    {
                        MaxLevel = Math.Max(MaxLevel, Building.GetUpgradeLevel());
                    }
                });

                return MaxLevel;
            }
        }
    }
}
