using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Place_Attacker : Command
    {
        internal override int Type => 700;

        public Place_Attacker(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal int X;
        internal int Y;
        internal CharacterData Character;

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.Character = this.Reader.ReadData<CharacterData>();
            base.Decode();
        }


        internal override void Execute()
        {
            if (this.Character != null)
            {
                var Level = this.Device.GameMode.Level;
                if (Level.GameObjectManager.Map == 0)
                {
                    this.Device.GameMode.Level.BattleManager.Battle.Add(this);
                    Item Unit = Level.Player.Units.GetByData(this.Character);

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
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"base", this.SaveBase()},
                {"x", this.X},
                {"y", this.Y},
                {"d", this.Character.GlobalId}
            };

            return Json;
        }
    }
}