using CR.Servers.CoC.Logic;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    internal class Disconnected : Message
    {
        internal override short Type => 25892;

        public Disconnected(Device Device) : base(Device)
        {
        }
        
        internal override void Process()
        {
            this.Device.State = State.DISCONNECTED;
        }
    }
}
