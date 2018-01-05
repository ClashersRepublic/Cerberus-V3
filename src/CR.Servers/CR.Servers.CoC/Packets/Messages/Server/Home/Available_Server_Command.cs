namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Available_Server_Command : Message
    {
        internal Command Command;

        public Available_Server_Command(Device Device) : base(Device)
        {
        }

        internal override short Type => 24111;

        internal override void Encode()
        {
            this.Data.AddInt(this.Command.Type);
            this.Command.Encode(this.Data);
        }
    }
}