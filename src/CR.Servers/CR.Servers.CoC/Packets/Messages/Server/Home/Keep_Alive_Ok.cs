using System;
using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    internal class Keep_Alive_Ok : Message
    {
        internal override short Type => 20108;

        public Keep_Alive_Ok(Device device) : base(device)
        {
        }
    }
}
