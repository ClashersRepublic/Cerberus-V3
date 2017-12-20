using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Friends_Online : Message
    {
        internal override short Type => 20206;

        public Avatar_Friends_Online(Device Device) : base(Device)
        {
            
        }

        internal override void Encode()
        {
            this.Data.AddLong(1);
            this.Data.AddVInt(1); 
            //1 = Player online
        }
    }
}
