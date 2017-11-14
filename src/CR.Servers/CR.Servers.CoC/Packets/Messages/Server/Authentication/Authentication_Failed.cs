using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, LoginFailedReason Reason = LoginFailedReason.Default) : base(Device)
        {
            this.Reason = Reason;
            Version = 9;
        }
        internal override short Type => 20103;

        internal LoginFailedReason Reason;


        internal string Message;
        internal string RedirectDomain;

        internal override void Encode()
        {
            Data.AddInt((int)Reason);
            Data.AddString(null);
            Data.AddString(RedirectDomain);
            Data.AddString(null);
            Data.AddString(null);
            Data.AddString(null);
            Data.AddInt(7000);
            Data.AddByte(0);
            Data.AddCompressed(null, false);
            Data.AddInt(-1);
            Data.AddInt(2);
            Data.AddInt(0);
            Data.AddInt(-1);
        }
    }
}
