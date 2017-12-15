using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Surrender_Attack : Command
    {
        internal override int Type => 703;

        public Surrender_Attack(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"t", this.ExecuteSubTick}               
            };
            return Json;
        }
    }
}
