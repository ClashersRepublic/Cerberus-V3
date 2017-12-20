using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Alliance_Stream_Entry_Removed : Message
    {
        internal override short Type => 24318;

        public Alliance_Stream_Entry_Removed(Device Device) : base(Device)
        {
            
        }

        internal long MessageId;

        internal override void Encode()
        {
            this.Data.AddLong(this.MessageId);
        }
    }
}
