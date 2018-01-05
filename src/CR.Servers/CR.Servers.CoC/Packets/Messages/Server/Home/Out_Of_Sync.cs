namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class Out_Of_Sync : Message
    {
        public Out_Of_Sync(Device Device) : base(Device)
        {
            // Out_Of_Sync_Message.
        }

        internal override short Type => 24104;

        internal override void Process()
        {
            this.Device.State = State.DISCONNECTED;
        }
    }
}