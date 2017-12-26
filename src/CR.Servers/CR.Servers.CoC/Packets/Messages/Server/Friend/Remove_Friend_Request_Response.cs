using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Friend
{
    internal class Remove_Friend_Request_Response : Message
    {
        internal override short Type => 20112;

        public Remove_Friend_Request_Response(Device Device) : base(Device)
        {
            
        }

        internal override void Encode()
        {
           this.Data.AddInt(1);
        }
    }
}
