using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Friend;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    internal class Remove_Friend : Message
    {
        internal override short Type => 10506;

        public Remove_Friend(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal long UserId;

        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            var Level = this.Device.GameMode.Level;
            var Friend = Level.Player.Friends.Get(this.UserId);

            if (Friend != null)
            {
                var Player = Friend.Player;

                if (Player != null)
                {
                    if (Level.Player.Friends.Remove(Friend, out _))
                    {
                        if (Player.Friends.Remove(Level.Player, out var LevelFriend))
                        {
                            Friend.State = FriendState.Removed;
                            new Friend_List_Entry(this.Device) {Friend = Friend}.Send();

                            if (Player.Connected)
                            {
                                LevelFriend.State = FriendState.Removed;
                                new Friend_List_Entry(Player.Level.GameMode.Device) {Friend = LevelFriend}
                                    .Send();
                            }
                        }
                        else
                            Logging.Error(this.GetType(), $"Unexpected issue while removing friend. Player Remove() function returned false!");
                    }
                    else
                        Logging.Error(this.GetType(), $"Unexpected issue while removing friend. Level Remove() function returned false!");
                }
                else
                    Logging.Error(this.GetType(), $"Unexpected issue while removing friend. Player is null!");
            }
            else
                Logging.Error(this.GetType(), $"Unexpected issue while removing friend. Friend is null!");
        }
    }
}
