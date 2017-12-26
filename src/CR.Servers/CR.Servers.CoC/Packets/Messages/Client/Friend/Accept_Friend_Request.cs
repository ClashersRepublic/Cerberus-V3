using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Friend;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    internal class Accept_Friend_Request : Message
    {
        internal override short Type => 10501;

        public Accept_Friend_Request(Device Device, Reader Reader) : base(Device, Reader)
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
            var LevelFriend = Level.Player.Friends.Get(this.UserId);

            if (LevelFriend != null)
            {
                var Player = LevelFriend.Player;

                if (Player != null)
                {
                    var PlayerFriend = Player.Friends.Get(Level.Player.UserId);

                    if (PlayerFriend != null)
                    {
                        LevelFriend.State = FriendState.Friend;
                        PlayerFriend.State = FriendState.Friend;

                        new Friend_List_Entry(this.Device) { Friend = LevelFriend }.Send();

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
