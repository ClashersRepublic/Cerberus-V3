using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Train_Unit_V2 : Command
    {
        internal int BuildingID;
        internal int Unknown1;
        internal Characters Unit;
        internal int Tick;
        public Train_Unit_V2(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        public override void Decode()
        {
            BuildingID = Reader.ReadInt32();
            Unknown1 = Reader.ReadInt32();
            Unit = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(Reader.ReadInt32()) as Characters;
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingID, true);
            if (go != null)
            {
                Builder_Building b = (Builder_Building)go;
                Unit_Storage_V2_Componenent c = b.GetUnitStorageV2Component();
                c?.AddUnit(Unit);
            }
        }
    }
}