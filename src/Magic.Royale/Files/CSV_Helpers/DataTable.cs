using System.Collections.Generic;
using Magic.Royale.Files.CSV_Logic;
using Magic.Royale.Files.CSV_Reader;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Files.CSV_Helpers
{
    internal class DataTable
    {
        internal List<Data> Datas;
        internal int Index;

        internal DataTable()
        {
            this.Datas = new List<Data>();
        }

        internal DataTable(Table Table, int Index)
        {
            this.Index = Index;
            this.Datas = new List<Data>();

            for (int i = 0; i < Table.GetRowCount(); i++)
            {
                Row Row = Table.GetRowAt(i);
                Data Data = this.Create(Row);

                this.Datas.Add(Data);
            }
        }

        internal Data Create(Row _Row)
        {
            Data _Data;

            switch ((Gamefile)this.Index)
            {
                case Gamefile.Treasure_Chests:
                    _Data = new Treasure_Chests(_Row, this);
                    break;
                case Gamefile.Rarities:
                    _Data = new Rarities(_Row, this);
                    break;
                case Gamefile.Spells_Characters:
                    _Data = new Spells_Characters(_Row, this);
                    break;
                case Gamefile.Spells_Buildings:
                    _Data = new Spells_Buildings(_Row, this);
                    break;
                case Gamefile.Spells_Other:
                    _Data = new Spells_Other(_Row, this);
                    break;
                case Gamefile.Arenas:
                    _Data = new Arenas(_Row, this);
                    break;

                default:
                {
                    _Data = new Data(_Row, this);
                    break;
                }
            }

            return _Data;
        }

        internal Data GetDataWithID(int ID)
        {
            int InstanceID = GlobalId.GetID(ID);
            return this.Datas[InstanceID];
        }

        internal Data GetDataWithInstanceID(int ID)
        {
            return this.Datas[ID];
        }

        internal Data GetData(string _Name)
        {
            return this.Datas.Find(_Data => _Data.Row.Name == _Name);
        }
    }
}