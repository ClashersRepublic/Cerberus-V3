using System.Collections.Generic;
using System.Windows;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Logic.Components;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Logic.Manager
{
    internal class ComponentManager
    {
        public ComponentManager(Level l)
        {
            m_vComponents = new List<List<Component>>();
            for (var i = 0; i <= 11; i++)
                m_vComponents.Add(new List<Component>());
            Level = l;
        }

        private readonly List<List<Component>> m_vComponents;

        private readonly Level Level;

        public void AddComponent(Component c)
        {
            m_vComponents[c.Type].Add(c);
        }

        public Component GetClosestComponent(int x, int y, Component_Filter cf)
        {
            Component result = null;
            var componentType = cf.Type;
            var components = m_vComponents[componentType];
            var v = new Vector(x, y);
            double maxLengthSquared = 0;

            if (components.Count > 0)
                foreach (var c in components)
                    if (cf.TestComponent(c))
                    {
                        var go = c.Parent;
                        var xy = go.GetPosition();
                        var lengthSquared = (v - new Vector(xy[0], xy[1])).LengthSquared;
                        if (lengthSquared < maxLengthSquared || result == null)
                        {
                            maxLengthSquared = lengthSquared;
                            result = c;
                        }
                    }
            return result;
        }

        public List<Component> GetComponents(int type)
        {
            return m_vComponents[type];
        }

        public int GetMaxSpellForgeLevel()
        {
            var result = 0;
            var components = m_vComponents[3];
            if (components.Count > 0)
                foreach (Unit_Production_Component c in components)
                    if (c.IsSpellForge)
                        if (c.Parent.ClassId == 0)
                        {
                            var b = (Building) c.Parent;
                            if (!b.IsConstructing || b.IsUpgrading)
                            {
                                var level = b.GetUpgradeLevel;
                                if (level > result)
                                    result = level;
                            }
                        }
                        else if (c.Parent.ClassId == 7)
                        {
                            var b = (Builder_Building) c.Parent;
                            if (!b.IsConstructing || b.IsUpgrading)
                            {
                                var level = b.GetUpgradeLevel;
                                if (level > result)
                                    result = level;
                            }
                        }
            return result;
        }

        public int GetMaxBarrackLevel()
        {
            var result = 0;
            var components = m_vComponents[3];
            if (components.Count > 0)
                foreach (Unit_Production_Component c in components)
                    if (!c.IsSpellForge)
                        if (c.Parent.ClassId == 0)
                        {
                            var level = ((Building) c.Parent).GetUpgradeLevel;
                            if (level > result)
                                result = level;
                        }
                        else if (c.Parent.ClassId == 7)
                        {
                            var level = ((Builder_Building) c.Parent).GetUpgradeLevel;
                            if (level > result)
                                result = level;
                        }
            return result;
        }

        public int GetTotalMaxHousingV2()
        {
            var result = 0;
            var components = m_vComponents[11];
            if (components.Count >= 1)
                foreach (var c in components)
                    result += ((Unit_Storage_V2_Componenent)c).MaxCapacity;
            return result;
        }

        public int GetTotalUsedHousingV2()
        {
            var result = 0;
            var components = m_vComponents[11];
            if (components.Count >= 1)
                foreach (var c in components)
                    result += ((Unit_Storage_V2_Componenent)c).GetUsedCapacity();
            return result;
        }
        /*

        public int GetTotalMaxHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = m_vComponents[0];
            if (components.Count >= 1)
                foreach (var c in components)
                    if (((UnitStorageComponent) c).IsSpellForge == IsSpellForge)
                        result += ((UnitStorageComponent) c).GetMaxCapacity();
            return result;
        }

        public int GetTotalUsedHousing(bool IsSpellForge = false)
        {
            var result = 0;
            var components = m_vComponents[0];
            if (components.Count >= 1)
                foreach (var c in components)
                    if (((UnitStorageComponent) c).IsSpellForge == IsSpellForge)
                        result += ((UnitStorageComponent) c).GetUsedCapacity();
            return result;
        }*/

        public void RefreshResourcesCaps()
        {
            var table = CSV.Tables.Get(Gamefile.Resources);
            var resourceCount = table.Datas.Count;
            var resourceStorageComponentCount = GetComponents(6).Count;
            for (var i = 0; i < resourceCount; i++)
            {
                var resourceCap = 0;
                for (var j = 0; j < resourceStorageComponentCount; j++)
                {
                    var res = (Resource_Storage_Component)GetComponents(6)[j];
                    if (res.IsEnabled)
                        resourceCap += res.GetMax(i);
                    var resource = (Files.CSV_Logic.Resource)table.Datas[i];
                    if (!resource.PremiumCurrency)
                    {
                       Level.Avatar.Resources_Cap.Set(resource.GetGlobalId(), resourceCap);
                    }
                }
            }
        }

        public void RemoveGameObjectReferences(Game_Object go)
        {
            foreach (var components in m_vComponents)
            {
                var markedForRemoval = new List<Component>();
                foreach (var component in components)
                    if (component.Parent == go)
                        markedForRemoval.Add(component);
                foreach (var component in markedForRemoval)
                    components.Remove(component);
            }
        }

        public void Tick()
        {
        }
    }
}
