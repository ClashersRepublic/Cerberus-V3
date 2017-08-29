using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Structure;
using Magic.ClashOfClans.Logic.Structure.Slots;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Remove_Units_V2 : Command
    {
        public Remove_Units_V2(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal int BuildingID;
        internal int Tick;

        public override void Decode()
        {
            BuildingID = Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var go = Device.Player.GameObjectManager.GetGameObjectByID(BuildingID, true);
            if (go != null)
            {
                var b = (Builder_Building) go;
                var c = b.GetUnitStorageV2Component();
                if (c != null)
                {
                    var playerunit = Device.Player.Avatar.Units2;

                    var _DataSlot = playerunit.Find(t => t.Data ==c .Units[0].Data.GetGlobalId());
                    if (_DataSlot != null)
                    {
                        _DataSlot.Count -= c.Units[0].Value;
                    }
                    c.Units = new List<DataSlot>();
                }

            }
        }
    }
}
