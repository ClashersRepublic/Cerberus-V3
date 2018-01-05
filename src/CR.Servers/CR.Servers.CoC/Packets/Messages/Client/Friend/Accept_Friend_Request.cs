namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using CR.Servers.CoC.Packets.Messages.Server.Friend;
    using CR.Servers.Extensions.Binary;

    internal class Accept_Friend_Request : Message
    {
        internal long UserId;

        public Accept_Friend_Request(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 10501;

        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();
        }


        internal override void Process()
        {
            Level Level = this.Device.GameMode.Level;
            Friend LevelFriend = Level.Player.Friends.Get(this.UserId);

            if (LevelFriend != null)
            {
                Player Player = LevelFriend.Player;

                if (Player != null)
                {
                    Friend PlayerFriend = Player.Friends.Get(Level.Player.UserId);

                    if (PlayerFriend != null)
                    {
                        LevelFriend.State = FriendState.Friend;
                        PlayerFriend.State = FriendState.Friend;

                        new Friend_List_Entry(this.Device) {Friend = LevelFriend}.Send();

                        if (Player.Connected)
                        {
                            new Friend_List_Entry(Player.Level.GameMode.Device) {Friend = PlayerFriend}.Send();
                        }
                    }
                }
            }
        }
    }
}