using System.Collections.Generic;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Files.CSV_Reader;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Files.CSV_Helpers
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

            switch ((Gamefile) Index)
            {

                case Gamefile.Buildings:
                    _Data = new Buildings(_Row, this);
                    break;
                case Gamefile.Resources:
                    _Data = new CSV_Logic.Resource(_Row, this);
                    break;
                case Gamefile.Characters:
                    _Data = new Characters(_Row, this);
                    break;
                case Gamefile.Obstacles:
                    _Data = new Obstacles(_Row, this);
                    break;
                case Gamefile.Experience_Levels:
                    _Data = new Experience_Levels(_Row, this);
                    break;
                case Gamefile.Traps:
                    _Data = new Traps(_Row, this);
                    break;
                case Gamefile.Globals:
                    _Data = new Globals(_Row, this);
                    break;
                case Gamefile.Npcs:
                    _Data = new Npcs(_Row, this);
                    break;
                case Gamefile.Decos:
                    _Data = new Decos(_Row, this);
                    break;
                case Gamefile.Missions:
                    _Data = new Missions(_Row, this);
                    break;
                case Gamefile.Spells:
                    _Data = new Spells(_Row, this);
                    break;
                case Gamefile.Heroes:
                    _Data = new Heroes(_Row, this);
                    break;
                case Gamefile.Leagues:
                    _Data = new Leagues(_Row, this);
                    break;
                case Gamefile.Variables:
                    _Data = new Variables(_Row, this);
                    break;
                case Gamefile.Village_Objects:
                    _Data = new Village_Objects(_Row, this);
                    break;
                /*case 2:
                    _Data = new Locales(_Row, this);
                    break;
                case 3:
                    _Data = new Resources(_Row, this);
                    break;
                case 4:
                    _Data = new Characters(_Row, this);
                    break;
                // case 5: Animation
                case 6:
                    _Data = new Projectiles(_Row, this);
                    break;
                case 7:
                    _Data = new Building_Classes(_Row, this);
                    break;
                case 8:
                    _Data = new Obstacles(_Row, this);
                    break;
                case 9:
                    _Data = new Effects(_Row, this);
                    break;
                // case 10: Particle Emitters
                case 11:
                    _Data = new Experience_Levels(_Row, this);
                    break;
                case 12:
                    _Data = new Traps(_Row, this);
                    break;
                case 13:
                    _Data = new Alliance_Badges(_Row, this);
                    break;
                case 14:
                    _Data = new Globals(_Row, this);
                    break;
                // case 15: TownHall Levels
                case 16:
                    _Data = new Alliance_Portal(_Row, this);
                    break;
                case 17:
                    _Data = new Npcs(_Row, this);
                    break;
                case 18:
                    _Data = new Decos(_Row, this);
                    break;
                // case 19: Resource Packs
                case 20:
                    _Data = new Shields(_Row, this);
                    break;
                // case 22: Billing Packages
                case 23:
                    _Data = new Achievements(_Row, this);
                    break;
                // case 24: Credits
                // case 25: Faq
                case 26:
                    _Data = new Spells(_Row, this);
                    break;
                // case 27: Hints
                case 28:
                    _Data = new Heroes(_Row, this);
                    break;
                case 29:
                    _Data = new Leagues(_Row, this);
                    break;

                case 37:
                    _Data = new Variables(_Row, this);
                    break;
                    */
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