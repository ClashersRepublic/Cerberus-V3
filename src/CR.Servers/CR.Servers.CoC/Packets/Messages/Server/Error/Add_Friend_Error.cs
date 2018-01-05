namespace CR.Servers.CoC.Packets.Messages.Server.Error
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Enums;
    using CR.Servers.Extensions.List;

    internal class Add_Friend_Error : Message
    {
        internal AddFriendErrorReason Reason;

        public Add_Friend_Error(Device Device) : base(Device)
        {
        }

        internal override short Type => 20112;

        internal override void Encode()
        {
            this.Data.AddInt((int) this.Reason);
        }
    }
}