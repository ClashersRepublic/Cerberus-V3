using System;
using System.Collections.Generic;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Files.CSV_Reader
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

        internal Data GetWithGlobalID(int GlobalID)
        {
            int Type = 0;

            while (GlobalID >= 1000000)
            {
                Type += 1;
                GlobalID -= 1000000;
            }

            try
            {
                return this.DataTables[Type].GetDataWithInstanceID(GlobalID);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ExceptionLogger.Log(e, $"Datatable throw ArgumentOutOfRangeException for {Type} with Global Id {GlobalID} ");
                return null;
            }
        }

        internal void Initialize(Table _Table, int _Index)
        {
            this.DataTables.Add(_Index, new DataTable(_Table, _Index));
        }
    }
}