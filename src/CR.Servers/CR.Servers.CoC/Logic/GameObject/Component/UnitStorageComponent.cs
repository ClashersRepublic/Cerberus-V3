using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    [Obsolete]
    internal class UnitStorageComponent : Component
    {
        internal int HousingSpace;
        internal int HousingSpaceAlt;

        internal List<Item> Units;

        internal int TotalUsedHousing
        {
            get
            {
                int Housing = 0;

                for (int i = 0; i < this.Units.Count; i++)
                {
                    if (this.Units[i].Count > 0)
                    {
                        Data Data = this.Units[i].Data;

                        if (Data.GetDataType() == 4)
                        {
                            Housing += ((CharacterData)Data).HousingSpace * this.Units[i].Count;
                        }
                    }
                }

                return Housing;
            }
        }

        internal override int Type => 0;

        public UnitStorageComponent(GameObject GameObject) : base(GameObject)
        {
            this.Units = new List<Item>(20);
            this.SetStorage();
        }

        internal void AddUnit(Data Data)
        {
            if (Data != null)
            {
                if (this.CanAddUnit(Data))
                {
                    Item Unit = this.Units.Find(T => T.Data == Data);

                    if (Unit != null)
                    {
                        ++Unit.Count;
                    }
                    else
                        this.Units.Add(new Item(Data, 1));
                }
               // else
                    //Logging.Info(this.GetType(), "AddUnit called and storage is full.");
            }
            //else
                //Logging.Info(this.GetType(), "AddUnit called with CharacterData NULL.");
        }

        internal bool CanAddUnit(Data Data)
        {
            if (Data.GetDataType() == 4)
            {
                CharacterData Character = (CharacterData)Data;

                if (this.HousingSpace >= this.TotalUsedHousing + Character.HousingSpace)
                {
                    return true;
                }
            }

            return false;
        }

        internal void SetStorage()
        {
            Building Building = (Building)this.Parent;

            if (!Building.Locked)
            {
                int Level = Building.GetUpgradeLevel();

                if (Level >= 0)
                {
                    this.HousingSpace = Building.BuildingData.HousingSpace[Level];
                    this.HousingSpaceAlt = Building.BuildingData.HousingSpaceAlt[Level];
                }
            }
        }

        internal override void Load(JToken Json)
        {
            JArray Units = (JArray)Json["units"];

            if (Units != null)
            {
                foreach (JToken Unit in Units)
                {
                    int[] Array = Unit.ToObject<int[]>();

                    int ID = Array[0];
                    int Count = Array[1];

                    if (ID != 0)
                    {
                        this.Units.Add(new Item(CSV.Tables.GetWithGlobalId(ID), Count));
                    }
                }
            }
        }

        internal override void Save(JObject Json)
        {
            JArray Units = new JArray();

            foreach (Item unit in this.Units)
            {
                Units.Add(new JArray
                {
                    unit.Data?.GlobalId ?? 0,
                    unit.Count
                });
            }

            Json.Add("units", Units);
            Json.Add("storage_type", 0);
        }
    }
}