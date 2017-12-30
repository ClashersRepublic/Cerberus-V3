using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Avatar;
using CR.Servers.CoC.Packets.Messages.Server.Friend;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Friend
{
    internal class Ask_For_Avatar_Friend_List : Message
    {
        internal override short Type => 10504;

        public Ask_For_Avatar_Friend_List(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Process()
        {
            new Friend_List(this.Device).Send();
        }
    }
}
