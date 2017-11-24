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

            switch ((Gamefile) Index)
            {
                case Gamefile.Buildings:
                    _Data = new BuildingData(_Row, this);
                    break;
                case Gamefile.Locales:
                    _Data = new LocaleData(_Row, this);
                    break;
                case Gamefile.Resources:
                    _Data = new ResourceData(_Row, this);
                    break;
                case Gamefile.Characters:
                    _Data = new CharacterData(_Row, this);
                    break;
                case Gamefile.Building_Classes:
                    _Data = new BuildingClassData(_Row, this);
                    break;
                case Gamefile.Obstacles:
                    _Data = new ObstacleData(_Row, this);
                    break;
                case Gamefile.Traps:
                    _Data = new TrapData(_Row, this);
                    break;
                case Gamefile.Globals:
                    _Data = new GlobalData(_Row, this);
                    break;
                case Gamefile.Experience_Levels:
                    _Data = new ExperienceLevelData(_Row, this);
                    break;
                case Gamefile.Townhall_Levels:
                    _Data = new TownhallLevelData(_Row, this);
                    break;
                case Gamefile.Npcs:
                    _Data = new NpcData(_Row, this);
                    break;
                case Gamefile.Missions:
                    _Data = new MissionData(_Row, this);
                    break;
                case Gamefile.Spells:
                    _Data = new SpellData(_Row, this);
                    break;
                case Gamefile.Heroes:
                    _Data = new HeroData(_Row, this);
                    break;
                case Gamefile.Variables:
                    _Data = new VariableData(_Row, this);
                    break;
                case Gamefile.Village_Objects:
                    _Data = new VillageObjectData(_Row, this);
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