using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Remove_Units : Command
    {
        public Remove_Units(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
        
        internal int Count;
        internal int Tick;
        internal List<int[]> UnitToRemove;

        public override void Decode()
        {
            Count = Reader.ReadInt32(); 
            UnitToRemove = new List<int[]>(Count);

            for (int i = 0; i < Count; i++)
            {
                Reader.ReadInt32();
                UnitToRemove.Add(new[]{ Reader.ReadInt32(), this.Reader.ReadInt32(), this.Reader.ReadInt32(), this.Reader.ReadInt32() });               
            }
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            foreach (var unit in UnitToRemove)
            {
                if (unit[0] == 0)
                {
                    List<Slot> _PlayerUnits = Device.Player.Avatar.Units;

                    var _DataSlot = _PlayerUnits.Find(t => t.Data == unit[1]);
                    if (_DataSlot != null)
                    {
                        _DataSlot.Count -= unit[2];
                    }
                }
                else
                {
                    List<Slot> _PlayerSpells = Device.Player.Avatar.Spells;

                    var _DataSlot = _PlayerSpells.Find(t => t.Data == unit[1]);
                    if (_DataSlot != null)
                        _DataSlot.Count -= unit[2];
                }
            }
        }
    }
}
