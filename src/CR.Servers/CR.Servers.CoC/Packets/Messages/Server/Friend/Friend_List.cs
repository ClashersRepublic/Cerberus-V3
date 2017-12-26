using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Friend
{
    internal class Friend_List : Message
    {
        internal override short Type => 20105;

        public Friend_List(Device Device) : base(Device)
        {
            
        }

        internal FriendListType ListType;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.ListType); //0 Normal, 1 Facebook, 2 Gamecenter, 3 Tencent
            this.Device.GameMode.Level.Player.Friends.Encode(this.Data);
        }
    }
}
