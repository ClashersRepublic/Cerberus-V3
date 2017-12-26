using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Friend;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    internal class Ask_For_Playing_Facebook_Friend_List : Message
    {
        internal override short Type => 10513;

        public Ask_For_Playing_Facebook_Friend_List(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Process()
        {
            //new Friend_List(this.Device) { ListType = FriendListType.Facebook}.Send();
        }
    }
}
