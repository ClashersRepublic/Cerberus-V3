namespace CR.Servers.CoC.Logic
{
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Helpers;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;

    internal class UnitSlots : DataSlots
    {
        internal UnitSlots(int Capacity) : base(Capacity)
        {
            // UnitSlots.
        }

        internal UnitSlots()
        {
            // UnitSlots.
        }

        internal int GetUnitsTotal()
        {
            int Total = 0;

            this.ForEach(Slot => { Total += Slot.Count; });

            return Total;
        }

        internal int GetUnitsTotalCapacity()
        {
            int Total = 0;

            this.ForEach(Slot =>
            {
                Data Data = CSV.Tables.GetWithGlobalId(Slot.Data);
                Total += Slot.Count * (Data.GetDataType() == 4 ? ((CharacterData) Data).HousingSpace : ((SpellData) Data).HousingSpace);
            });

            return Total;
        }
    }
}