using System.Collections.Generic;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Extensions.List;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class AllianceUnitSlots : List<UnitItem>
    {
        internal int Checksum
        {
            get
            {
                int Checksum = 0;

                this.ForEach(Item =>
                {
                    Checksum += Item.Checksum;
                });

                return Checksum;
            }
        }

        public AllianceUnitSlots(int Capacity = 50) : base(Capacity)
        {
            // AllianceUnitSlots.
        }

        internal void Add(Data Data, int Count, int Level)
        {
            if (this.TryGet(T => T.Data == Data && T.Level == Level, out UnitItem Current))
            {
                Current.Count += Count;
            }
            else
                base.Add(new UnitItem(Data, Count, Level));
        }

        internal new void Add(UnitItem Item)
        {
            if (this.TryGet(T => T.Data == Item.Data && T.Level == Item.Level, out UnitItem Current))
            {
                Current.Count += Item.Count;
            }
            else
                base.Add(Item);
        }

        internal Item GetByData(Data Data, int Level)
        {
            return this.Find(T => T.Data == Data && T.Level == Level);
        }

        internal Item GetByGlobalId(int Id, int Level)
        {
            return this.Find(T => T.Data.GlobalId == Id && T.Level == Level);
        }

        internal int GetCountByData(Data Data, int Level)
        {
            return this.Find(T => T.Data == Data && T.Level == Level)?.Count ?? 0;
        }

        internal int GetCountByGlobalId(int Id, int Level)
        {
            return this.Find(T => T.Data.GlobalId == Id && T.Level == Level)?.Count ?? 0;
        }

        internal void Remove(Data Data, int Count, int Level)
        {
            if (this.TryGet(T => T.Data == Data && T.Level == Level, out UnitItem Current))
            {
                Current.Count -= Count;
            }
        }

        internal void Remove(Item Item, int Count, int Level)
        {
            if (this.TryGet(T => T.Data == Item.Data && T.Level == Level, out UnitItem Current))
            {
                Current.Count -= Count;

                if (Current.Count < 0)
                {
                    //Logging.Info(this.GetType(), "Remove() - Count is inferior at 0. This should never happen, check count before remove.");
                    Current.Count = 0;
                }
            }
            //else
                //Logging.Info(this.GetType(), "Remove() - DataSlot doesn't exist. This should never happen, check existing before remove.");
        }

        internal void Set(int Id, int Count, int Level)
        {
            this.Set(CSV.Tables.GetWithGlobalId(Id), Count, Level);
        }

        internal void Set(Data Data, int Count, int Level)
        {
            if (this.TryGet(T => T.Data == Data && T.Level == Level, out UnitItem Current))
            {
                Current.Count = Count;
            }
            else
                this.Add(new UnitItem(Data, Count, Level));
        }

        internal void Set(UnitItem Item)
        {
            if (this.TryGet(T => T.Data == Item.Data && T.Level == Item.Level, out UnitItem Current))
            {
                Current.Count = Item.Count;
            }
            else
                this.Add(Item);
        }

        internal void Encode(List<byte> Packet)
        {
            Packet.AddInt(this.Count);

            this.ForEach(Item =>
            {
                Item.Encode(Packet);
            });
        }

        internal JArray Save()
        {
            JArray Array = new JArray();

            this.ForEach(Item =>
            {
                Array.Add(Item.Save());
            });

            return Array;
        }
    }
}