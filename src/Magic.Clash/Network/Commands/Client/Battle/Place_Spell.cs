using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    internal class Place_Spell : Command
    {
        internal int GlobalId;
        internal int X;
        internal int Y;
        internal byte Unknown_Byte;
        internal int Unknown_Int;
        internal int Tick;

        internal Spells Spell;

        public Place_Spell(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            X = Reader.ReadInt32();
            Y = Reader.ReadInt32();
            GlobalId = Reader.ReadInt32();
            Spell = CSV.Tables.Get(Gamefile.Spells).GetDataWithID(GlobalId) as Spells;
            Unknown_Byte = Reader.ReadByte();
            Unknown_Int = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.State == State.IN_PC_BATTLE)
            {
            }
            List<Slot> _PlayerSpells = Device.Player.Avatar.Spells;

            var _DataSlot = _PlayerSpells.Find(t => t.Data == GlobalId);
            if (_DataSlot != null)
                _DataSlot.Count -= 1;
        }
    }
}
