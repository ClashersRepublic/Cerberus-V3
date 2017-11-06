using System.Collections.Generic;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Files.CSV_Reader;

namespace CR.Servers.CoC.Files.CSV_Helpers
{
    internal class DataTable
    {
        internal List<Data> Datas;
        internal int Index;

        internal DataTable()
        {
            Datas = new List<Data>();
        }

        internal DataTable(Table Table, int Index)
        {
            this.Index = Index;
            Datas = new List<Data>();

            for (int i = 0; i < Table.GetRowCount(); i++)
            {
                Row Row = Table.GetRowAt(i);
                Data Data = Create(Row);

                Datas.Add(Data);
            }
        }

        internal Data Create(Row _Row)
        {
            Data _Data;

            switch ((Gamefile)Index)
            {
                case Gamefile.Buildings:
                    _Data = new BuildingData(_Row, this);
                    break;
                case Gamefile.Resources:
                    _Data = new ResourceData(_Row, this);
                    break;
                case Gamefile.Building_Classes:
                    _Data = new BuildingClassData(_Row, this);
                    break;

                default:
                    _Data = new Data(_Row, this);
                    break;
            }

            return _Data;
        }

        internal Data GetDataWithID(int ID)
        {
            int InstanceID = GlobalId.GetID(ID);
            return Datas[InstanceID];
        }

        internal Data GetDataWithInstanceID(int ID)
        {
            return Datas[ID];
        }

        internal Data GetData(string _Name)
        {
            return Datas.Find(_Data => _Data.Row.Name == _Name);
        }
    }
}