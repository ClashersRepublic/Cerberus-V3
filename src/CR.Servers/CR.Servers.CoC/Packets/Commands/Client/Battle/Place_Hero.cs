using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Place_Hero : Command
    {
        internal override int Type => 705;

        public Place_Hero(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int X;
        internal int Y;
        internal HeroData Hero;

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.Hero = this.Reader.ReadData<HeroData>();
            base.Decode();
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"base", this.SaveBase()},
                {"x", this.X},
                {"y", this.Y},
                {"d", this.Hero.GlobalId}
            };

            return Json;
        }
    }
}
