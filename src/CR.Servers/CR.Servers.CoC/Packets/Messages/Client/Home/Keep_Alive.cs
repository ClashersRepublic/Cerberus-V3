using System;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    internal class Keep_Alive : Message
    {
        internal override short Type => 10108;

        public Keep_Alive(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Process()
        {
            new Keep_Alive_Ok(this.Device).Send();
            this.Device.LastKeepAlive = DateTime.UtcNow;
        }
    }
}