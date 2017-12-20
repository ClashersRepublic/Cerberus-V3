using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Avatar;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Avatar
{
    internal class Ask_For_Avatar_Friend_List : Message
    {
        internal override short Type => 10504;

        public Ask_For_Avatar_Friend_List(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Process()
        {
            this.Device.GameMode.Level.Player.Variables.Set(Variable.FriendListLastOpened, TimeUtils.UnixUtcNow);
            /*new Avatar_Friends_Online(this.Device).Send();
            new Avatar_Friend_List(this.Device)
            {
                Friends = new List<Player>()
                {
                    Resources.Accounts.LoadAccount(0,2).Player
                }
            }.Send();*/
            

        }
    }
}
