﻿using System.Collections.Generic;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Files.CSV_Reader;

namespace CR.Servers.CoC.Files.CSV_Logic.Logic
{
    internal class TownhallLevelData : Data
    {
        internal Dictionary<Data, int> Caps;

        public TownhallLevelData(Row Row, DataTable DataTable) : base(Row, DataTable)
        {
        }

        internal override void Process()
        {
            this.Caps = new Dictionary<Data, int>(64);

            /*this.LoadTable(CSV.Tables.Get(Gamefile.Building));
            this.LoadTable(CSV.Tables.Get(Gamefile.Trap));
            this.LoadTable(CSV.Tables.Get(Gamefile.Deco));*/
        }

        public string Name { get; set; }
        public int AttackCost { get; set; }
        public int ResourceStorageLootPercentage { get; set; }
        public int DarkElixirStorageLootPercentage { get; set; }
        public int ResourceStorageLootCap { get; set; }
        public int DarkElixirStorageLootCap { get; set; }
        public int WarPrizeResourceCap { get; set; }
        public int WarPrizeDarkElixirCap { get; set; }
        public int WarPrizeAllianceExpCap { get; set; }
        public int CartLootCapResource { get; set; }
        public int CartLootReengagementResource { get; set; }
        public int CartLootCapDarkElixir { get; set; }
        public int CartLootReengagementDarkElixir { get; set; }

        /*private void LoadTable(DataTable Table)
        {
            Table.Datas.ForEach(Data =>
            {
                string Value = Row.GetValue(Data.Name, this.DataTable.Datas.Count);

                if (!string.IsNullOrEmpty(Value))
                {
                    if (int.TryParse(Value, out int Count))
                    {
                        this.Caps.Add(Data, Count);
                    }
                    //else
                        //Logging.Error(this.GetType(), "Value " + Value + " is not int value.");
                }
                else if (this.DataTable.Datas.Count > 0)
                {
                    int Count = 0;

                    foreach (TownhallLevelData TData in this.DataTable.Datas)
                    {
                        if (TData.Caps[Data] > Count)
                        {
                            Count = TData.Caps[Data];
                        }
                    }

                    this.Caps.Add(Data, Count);
                }
                else
                    this.Caps.Add(Data, 0);
            });
        }*/

    }
}
