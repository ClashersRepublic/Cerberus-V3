using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Builder_Building : Construction_Item
    {
        public Builder_Building(Data data, Level level) : base(data, level)
        {
            if (GetBuildingData.BuildingClass == "Defense" || GetBuildingData.BuildingClass == "Wall")
            {
                AddComponent(new Combat_Component(this));
            }
            if (GetBuildingData.IsHeroBarrack)
            {
                Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(GetBuildingData.HeroType) as Heroes;
                AddComponent(new Hero_Base_Component(this, hd));
            }
            if (GetBuildingData.UpgradesUnits)
                AddComponent(new Unit_Upgrade_Component(this));
            if (GetBuildingData.UnitProduction[0] > 0)
            {
                AddComponent(new Unit_Production_Component(this));
            }
            if (!string.IsNullOrEmpty(GetBuildingData.ProducesResource))
            {
                AddComponent(new Resource_Production_Component(this, level));
            }

            if (GetBuildingData.MaxStoredElixir2[0] > 0 || GetBuildingData.MaxStoredGold2[0] > 0)
            {
                AddComponent(new Resource_Storage_Component(this));
            }
            if (GetBuildingData.Village2Housing > 0)
            {
                AddComponent(new Unit_Storage_V2_Componenent(this, 0));
            }
            /*AddComponent(new Hitpoint_Component());*/
        }

        internal override bool Builder => true;
        internal override int ClassId => 7;
        internal override int OppositeClassId => 0;

        public Buildings GetBuildingData => (Buildings) Data;
    }
}