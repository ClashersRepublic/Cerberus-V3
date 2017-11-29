using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Place_Spell :Command
    {
        internal override int Type => 704;

        public Place_Spell(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal int X;
        internal int Y;
        internal SpellData Spell;
        internal byte UnknownByte;
        internal int UnknownInt;

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.Spell = this.Reader.ReadData<SpellData>();
            this.UnknownByte = this.Reader.ReadByte();
            this.UnknownInt = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            ShowValues();
            if (this.Spell != null)
            {
                var Level = this.Device.GameMode.Level;
                Item Unit = Level.Player.Spells.GetByData(this.Spell);

                if (Unit != null)
                {
                    if (Unit.Count > 0)
                    {
                        //Do some logging shit for replay and etc

                        Unit.Count--;
                    }
                }
            }
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"base", this.SaveBase()},
                {"x", this.X},
                {"y", this.Y},
                {"d", this.Spell.GlobalId}
            };

            return Json;
        }
    }
}