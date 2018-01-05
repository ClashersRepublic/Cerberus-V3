namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class Disconnected : Message
    {
        public Disconnected(Device Device) : base(Device)
        {
        }

        internal override short Type => 25892;

        internal override void Process()
        {
            this.Device.State = State.DISCONNECTED;
        }
    }
}