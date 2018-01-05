namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class Ask_For_Playing_Facebook_Friend_List : Message
    {
        public Ask_For_Playing_Facebook_Friend_List(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 10513;

        internal override void Process()
        {
            //new Friend_List(this.Device) { ListType = FriendListType.Facebook}.Send();
        }
    }
}