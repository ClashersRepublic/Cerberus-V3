using System;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
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
            this.Device.KeepAliveOk.Send();
            this.Device.LastKeepAlive = DateTime.UtcNow;
        }
    }
}