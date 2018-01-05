namespace CR.Servers.CoC.Packets.Messages.Server.Friend
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Remove_Friend_Request_Response : Message
    {
        public Remove_Friend_Request_Response(Device Device) : base(Device)
        {
        }

        internal override short Type => 20112;

        internal override void Encode()
        {
            this.Data.AddInt(1);
        }
    }
}