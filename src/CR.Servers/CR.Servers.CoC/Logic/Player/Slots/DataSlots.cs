using System;
using System.Collections.Generic;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class DataSlots : List<Item>
    {
        internal int Checksum
        {
            get
            {
                var Checksum = 0;

                this.ForEach(Item =>
                {
                    Checksum += Item.Checksum;
                });

                return Checksum;
            }
        }

        public DataSlots(int Capacity = 50) : base(Capacity)
        {
            // DataSlots.
        }

        public DataSlots() 
        {
        }

        internal void Add(Data Data, int Count)
        {
            if (this.TryGet(T => T.Data == Data.GlobalId, out Item Current))
            {
                Current.Count += Count;
            }
            else
                base.Add(new Item(Data.GlobalId, Count));
        }

        internal void Add(int Data, int Count)
        {
            if (this.TryGet(T => T.Data == Data, out Item Current))
            {
                Current.Count += Count;
            }
            else
                base.Add(new Item(Data, Count));
        }

        internal new void Add(Item Item)
        {
            if (this.TryGet(T => T.Data == Item.Data, out Item Current))
            {
                Current.Count += Item.Count;
            }
            else
                base.Add(Item);
        }

        internal Item GetByData(Data Data)
        {
            return this.Find(T => T.Data == Data.GlobalId);
        }

        internal Item GetByGlobalId(int Id)
        {
            return this.Find(T => T.Data == Id);
        }

        internal int GetCountByData(Data Data)
        {
            return this.Find(T => T.Data == Data.GlobalId)?.Count ?? 0;
        }

        internal int GetCountByGlobalId(int Id)
        {
            return this.Find(T => T.Data == Id)?.Count ?? 0;
        }

        internal void Remove(Data Data, int Count)
        {
            if (this.TryGet(T => T.Data == Data.GlobalId, out Item Current))
            {
                Current.Count -= Count;
            }
#if DEBUG
            //else if (Count > 0)
                //Logging.Info(this.GetType(), "Remove() - DataSlot doesn't exist. This should never happen, check existing before remove. (" + Data.GlobalID + ")");
#endif
        }

        internal void Remove(Item Item, int Count)
        {
            if (this.TryGet(T => T.Data == Item.Data, out Item Current))
            {
                Current.Count -= Count;

                if (Current.Count < 0)
                {
                    //Logging.Info(this.GetType(), "Remove() - Count is inferior at 0. This should never happen, check count before remove.");
                    Current.Count = 0;
                }
            }
#if DEBUG
            //else if (Count > 0)
              //  Logging.Info(this.GetType(), "Remove() - DataSlot doesn't exist. This should never happen, check existing before remove.");
#endif
        }

        internal void Set(int Id, int Count)
        {
            if (this.TryGet(T => T.Data == Id, out Item Current))
            {
                Current.Count = Count;
            }
            else
                base.Add(new Item(Id, Count));
        }

        internal void Set(Data Data, int Count)
        {
            if (this.TryGet(T => T.Data == Data.GlobalId, out Item Current))
            {
                Current.Count = Count;
            }
            else
                base.Add(new Item(Data.GlobalId, Count));
        }

        internal void Set(Item Item)
        {
            if (this.TryGet(T => T.Data == Item.Data, out Item Current))
            {
                Current.Count = Item.Count;
            }
            else
                base.Add(Item);
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