namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    using CR.Servers.CoC.Logic;

    internal class Keep_Alive_Ok : Message
    {
        public Keep_Alive_Ok(Device device) : base(device)
        {
        }

        internal override short Type => 20108;
    }
}