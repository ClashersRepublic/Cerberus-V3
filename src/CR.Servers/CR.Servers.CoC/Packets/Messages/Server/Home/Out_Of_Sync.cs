using CR.Servers.CoC.Logic;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    internal class Out_Of_Sync : Message
    {
        internal override short Type => 24104;

        public Out_Of_Sync(Device Device) : base(Device)
        {
            // Out_Of_Sync_Message.
        }

        internal override void Process()
        {
            this.Device.State = State.DISCONNECTED;
        }
    }
}
