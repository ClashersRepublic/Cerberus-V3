using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    internal class Place_Attacker : Command
    {
        internal Characters Troop;

        internal int GlobalId;
        internal int X;
        internal int Y;
        internal int Tick;


        public Place_Attacker(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            X = Reader.ReadInt32();
            Y = Reader.ReadInt32();
            GlobalId = Reader.ReadInt32();
            Troop = CSV.Tables.Get(Gamefile.Characters).GetDataWithID(GlobalId) as Characters;

            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.Player.Avatar.Variables.IsBuilderVillage)
            {
                /*List<Component> components = this.Device.Player.GetComponentManager().GetComponents(11);

                foreach (Component t in components)
                {
                    Unit_Storage_V2_Componenent c = (Unit_Storage_V2_Componenent)t;
                    if (c.GetUnitTypeIndex(this.Troop) != -1)
                    {
                        var storageCount = c.GetUnitCountByData(this.Troop);
                        if (storageCount >= 1)
                        {
                            c.RemoveUnits(this.Troop, 1);
                            break;
                        }
                    }
                }*/
            }
            else
            {
                List<Slot> _PlayerUnits = Device.Player.Avatar.Units;

                var _DataSlot = _PlayerUnits.Find(t => t.Data == GlobalId);
                if (_DataSlot != null)
                    _DataSlot.Count -= 1;
            }

            if (Device.State == State.IN_PC_BATTLE)
            {
                if (!Device.Player.Avatar.Variables.IsBuilderVillage && !Device.Player.Avatar.Modes.IsAttackingOwnBase)
                {
                }
            }
            else if (Device.State == State.IN_1VS1_BATTLE)
            {
            }
        }
    }
}