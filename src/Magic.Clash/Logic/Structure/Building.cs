using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Building : Construction_Item
    {
        public Building(Data data, Level level) : base(data, level)
        {
            if (GetBuildingData.BuildingClass == "Defense")
                AddComponent(new Combat_Component(this));
            if (GetBuildingData.IsHeroBarrack)
            {
                var hd = CSV.Tables.Get(Gamefile.Heroes).GetData(GetBuildingData.HeroType) as Heroes;
                AddComponent(new Hero_Base_Component(this, hd));
            }
            if (GetBuildingData.UpgradesUnits)
                AddComponent(new Unit_Upgrade_Component(this));

            if (GetBuildingData.UnitProduction[0] > 0)
                AddComponent(new Unit_Production_Component(this));
            /*AddComponent(new Hitpoint_Component());


            //if (GetBuildingData.HousingSpace[0] > 0)
            {
                //AddComponent(new UnitStorageComponent(this, 0));
            }

            if (!string.IsNullOrEmpty(GetBuildingData.ProducesResource))
            {
                AddComponent(new Resource_Production_Component(this, level));
            }

            if (GetBuildingData.MaxStoredGold[0] > 0 || GetBuildingData.MaxStoredElixir[0] > 0 || GetBuildingData.MaxStoredDarkElixir[0] > 0 || GetBuildingData.MaxStoredWarGold[0] > 0 || GetBuildingData.MaxStoredWarElixir[0] > 0 || GetBuildingData.MaxStoredWarDarkElixir[0] > 0)
            {
                AddComponent(new Resource_Storage_Component(this));
            }*/
        }


        internal override bool Builder => false;
        internal override int ClassId => 0;
        internal override int OppositeClassId => 7;

        public Buildings GetBuildingData => (Buildings) Data;
    }
}
