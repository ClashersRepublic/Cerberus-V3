namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Friend;
    using CR.Servers.Extensions.Binary;

    internal class Ask_For_Avatar_Friend_List : Message
    {
        public Ask_For_Avatar_Friend_List(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 10504;

        internal override void Process()
        {
            new Friend_List(this.Device).Send();
        }
    }
}