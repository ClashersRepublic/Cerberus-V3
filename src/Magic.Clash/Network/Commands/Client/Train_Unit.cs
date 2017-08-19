using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Train_Unit : Command
    {
        internal Characters Troop;
        internal Spells Spell;

        internal int GlobalId;
        internal int Count;
        internal int Tick;

        internal bool IsSpell;

        public Train_Unit(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Reader.ReadInt32();
            Reader.ReadUInt32();
            GlobalId = Reader.ReadInt32();
            if (GlobalId >= 26000000)
            {
                IsSpell = true;
                Spell = CSV.Tables.Get(Gamefile.Spells).GetDataWithID(GlobalId) as Spells;
            }
            else
            {
                Troop = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(GlobalId) as Characters;
            }
            Count = Reader.ReadInt32();
            Reader.ReadUInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var _Player = Device.Player.Avatar;

            if (IsSpell)
                _Player.Add_Spells(Spell.GetGlobalId(), Count);
            else
                _Player.Add_Unit(Troop.GetGlobalId(), Count);
        }
    }
}
