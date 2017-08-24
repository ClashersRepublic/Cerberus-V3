using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Reader;

namespace Magic.ClashOfClans.Files.CSV_Logic
{
    internal class Construction_Item_Data : Data
    {
        public Construction_Item_Data(Row row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetBuildCost(int level) => -1;

        public virtual Resource GetBuildResource(int level) => null;

        public virtual int GetConstructionTime(int level) => -1;

        public virtual int GetGearUpTime(int level) => -1;

        public virtual int GetRequiredTownHallLevel(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual bool IsTownHall() => false;
        public virtual bool IsTownHall2() => false;
        public virtual bool IsAllianceCastle() => false;
    }

}
