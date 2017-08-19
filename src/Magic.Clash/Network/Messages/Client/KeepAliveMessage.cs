using System;
using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;
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