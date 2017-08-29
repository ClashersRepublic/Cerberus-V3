using System;
using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Structure;
using Magic.ClashOfClans.Logic.Structure.Slots;
using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Unit_Storage_V2_Componenent : Component
    {
        internal List<DataSlot> Units;
        internal int MaxCapacity;
        public override int Type => 11;

        public Unit_Storage_V2_Componenent(Game_Object go, int capacity) : base(go)
        {
            Units = new List<DataSlot>();
            MaxCapacity = capacity;
        }

        internal void AddUnit(Combat_Item cd)
        {
            AddUnitImpl(cd);
        }

        internal bool CanAddUnit(Combat_Item cd)
        {
            var result = false;
            if (cd != null)
            {
                var cm = Parent.Level.GetComponentManager();
                var maxCapacity = cm.GetTotalMaxHousingV2();
                var usedCapacity = cm.GetTotalUsedHousingV2();
                var housingSpace = cd.GetHousingSpace();
                if (GetUsedCapacity() < MaxCapacity)
                    result = maxCapacity >= usedCapacity + housingSpace;  //TODO: Make fully working
            }
            return result;
        }

        internal void AddUnitImpl(Combat_Item cd)
        {
            //if (CanAddUnit(cd))
            {
                var ca = Parent.Level.Avatar;
                var UnitInCamp = ((Characters) cd).UnitsInCamp[ca.GetUnitUpgradeLevel(cd)];
                var unitIndex = GetUnitTypeIndex(cd);
                if (unitIndex == -1)
                {
                    var us = new DataSlot(cd, UnitInCamp);
                    Units.Add(us);
                }
                else
                {
                    Units[unitIndex].Value += UnitInCamp;
                }
                var unitCount = ca.Get_Unit_Count_V2(cd);
                ca.Set_Unit_Count_V2(cd, unitCount + UnitInCamp);
            }
        }

        public int GetUnitCountByData(Combat_Item cd)
        {
            return Units.Where(t => t.Data == cd).Sum(t => t.Value);
        }

        internal int GetUnitTypeIndex(Combat_Item cd)
        {
            var index = -1;
            for (var i = 0; i < Units.Count; i++)
                if (Units[i].Data == cd)
                {
                    index = i;
                    break;
                }
            return index;
        }

        internal int GetUsedCapacity()
        {
            var count = 0;
            if (Units.Count >= 1)
                count += (from t in Units
                    let cnt = t.Value
                    let housingSpace = ((Combat_Item) t.Data).GetHousingSpace()
                    select cnt * housingSpace).Sum();
            return count;
        }

        public void RemoveUnits(Combat_Item cd, int count)
        {
            RemoveUnitsImpl(cd, count);
        }

        public void RemoveUnitsImpl(Combat_Item cd, int count)
        {
            var unitIndex = GetUnitTypeIndex(cd);
            if (unitIndex != -1)
            {
                var us = Units[unitIndex];
                if (us.Value <= count)
                    Units.Remove(us);
                else
                    us.Value -= count;
                var ca = Parent.Level.Avatar;
                var unitCount = ca.Get_Unit_Count_V2(cd);
                ca.Set_Unit_Count_V2(cd, unitCount - count);
            }
        }

        public override void Load(JObject jsonObject)
        {
            var unitObject = (JObject) jsonObject["up2"];
            var unitArray = (JArray) unitObject?["unit"];
            if (unitArray?.Count > 0)
            {
                var id = unitArray[0].ToObject<int>();
                var cnt = unitArray[1].ToObject<int>();
                Units.Add(new DataSlot((Combat_Item) CSV.Tables.GetWithGlobalID(id), cnt));
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            var unitObject = new JObject();
            var unitJsonArray = new JArray();
            if (Units.Count > 0)
            {
                foreach (var unit in Units)
                    unitJsonArray = new JArray {unit.Data.GetGlobalId(), unit.Value};
                unitObject.Add("unit", unitJsonArray);
            }
            jsonObject.Add("up2", unitObject);
            return jsonObject;
        }
    }
}
