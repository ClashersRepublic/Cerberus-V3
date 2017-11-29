using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Hero_Rage : Command
    {
        internal override int Type => 706;

        public Hero_Rage(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int GlobalId;

        internal override void Decode()
        {
            this.GlobalId = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"base", this.SaveBase()},
                { "d", this.GlobalId}
            };

            return Json;
        }
    }
}
