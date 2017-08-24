using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Server_Commands : Message
    {
        internal Command Command;

        public Server_Commands(Device Device, Command Command) : base(Device)
        {
            Identifier = 24111;
            this.Command = Command.Handle();
        }

        public Server_Commands(Device device) : base(device)
        {
            Identifier = 24111;
        }

        public override void Encode()
        {
            Data.AddInt(Command.Identifier);
            Data.AddRange(Command.Data);
        }
    }
}