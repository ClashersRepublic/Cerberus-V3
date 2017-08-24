using System;
using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(Device device, Reader reader) : base(device, reader)
        {
            // Space
        }

        public override void Process()
        {
            Device.LastKeepAlive = DateTime.Now;
            Device.NextKeepAlive = Device.LastKeepAlive.AddSeconds(30);
            Device._keepAliveOk.Send();
        }
    }
}