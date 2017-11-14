using System.Collections.Generic;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class UnitItem : Item
    {
        [JsonProperty] internal int Level;

        internal override int Checksum => base.Checksum + this.Level;

        public UnitItem() : base()
        {

        }

        public UnitItem(Data Data, int Count, int Level) : base(Data, Count)
        {
            this.Level = Level;
        }

        internal override void Decode(Reader Reader)
        {
            base.Decode(Reader);
            this.Level = Reader.ReadInt32();
        }

        internal override void Encode(List<byte> Writer)
        {
            base.Encode(Writer);
            Writer.AddInt(this.Level);
        }

        internal override JObject Save()
        {
            var Json = base.Save();

            Json.Add("lvl", this.Level);

            return Json;
        }

        public override bool Equals(object obj)
        {
            var Item = obj as UnitItem;

            if (Item != null)
            {
                return Item.Data == this.Data && Item.Level == this.Level;
            }

            return false;
        }
    }
}