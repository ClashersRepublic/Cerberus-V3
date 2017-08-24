using System;
using Magic.Royale.Extensions.Binary;

namespace Magic.Royale.Network.Messages.Client
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