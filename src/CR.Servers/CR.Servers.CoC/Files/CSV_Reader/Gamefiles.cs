using System;
using System.Collections.Generic;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Files.CSV_Reader;

namespace CR.Servers.CoC.Files.CSV_Reader
{
    internal class Gamefiles
    {
        internal readonly Dictionary<int, DataTable> DataTables;

        internal Gamefiles()
        {
            this.DataTables = new Dictionary<int, DataTable>(CSV.Gamefiles.Count);
        }

        internal DataTable Get(int _Index)
        {
            return this.DataTables[_Index];
        }

        internal DataTable Get(Gamefile _Index)
        {
            return this.DataTables[(int)_Index];
        }

        internal Data GetWithGlobaId(int GlobalId)
        {
            int Class = GlobalId / 1000000;
            int Instance = GlobalId % 1000000;

            if (this.DataTables.TryGetValue(Class, out DataTable Table))
            {
                if (Table.Datas.Count > Instance)
                {
                    return Table.Datas[Instance];
                }
            }

            return null;
        }

        internal void Initialize(Table Table, int Index)
        {
            this.DataTables.Add(Index, new DataTable(Table, Index));
        }
    }
}
