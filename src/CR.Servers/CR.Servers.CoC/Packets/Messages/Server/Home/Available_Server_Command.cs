using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    internal class Available_Server_Command : Message
    {
        internal Command Command;

        internal override short Type => 24111;

        public Available_Server_Command(Device Device) : base(Device)
        {
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.Command.Type);
            this.Command.Encode(this.Data);
        }
    }
}
