namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    using CR.Servers.CoC.Logic;

    internal class Village2AttackEntryUpdateMessage : Message
    {
        public Village2AttackEntryUpdateMessage(Device Device) : base(Device)
        {
        }

        internal override short Type
        {
            get
            {
                return 24371;
            }
        }
    }
}