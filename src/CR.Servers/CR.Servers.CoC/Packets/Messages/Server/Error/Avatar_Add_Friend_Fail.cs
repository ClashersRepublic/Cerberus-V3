namespace CR.Servers.CoC.Packets.Messages.Server.Error
{
    using CR.Servers.CoC.Logic;

    internal class Avatar_Add_Friend_Fail : Message
    {
        public Avatar_Add_Friend_Fail(Device Device) : base(Device)
        {
        }

        internal override short Type => 20112;
    }
}