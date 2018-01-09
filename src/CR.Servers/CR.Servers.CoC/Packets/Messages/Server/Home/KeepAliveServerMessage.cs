namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    using CR.Servers.CoC.Logic;

    internal class KeepAliveServerMessage : Message
    {
        public KeepAliveServerMessage(Device device) : base(device)
        {
        }

        internal override short Type => 20108;
    }
}