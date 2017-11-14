using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;

namespace CR.Servers.CoC.Logic
{
    internal class UnitSlots : DataSlots
    {
        internal UnitSlots(int Capacity = 10) : base(Capacity)
        {
            // UnitSlots.
        }

        internal int GetUnitsTotal()
        {
            int Total = 0;

            this.ForEach(Slot =>
            {
                Total += Slot.Count;
            });

            return Total;
        }

        internal int GetUnitsTotalCapacity()
        {
            int Total = 0;

            this.ForEach(Slot =>
            {
                var Data = CSV.Tables.GetWithGlobalId(Slot.Data);
                Total += Slot.Count * (Data.GetDataType() == 4 ? ((CharacterData)Data).HousingSpace : 0);
            });

            return Total;
        }
    }
}