using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Messages.Server.Error
{
    internal class Avatar_Add_Friend_Fail : Message
    {
        internal override short Type => 20112;

        public Avatar_Add_Friend_Fail(Device Device) : base(Device)
        {
            
        }
    }
}
