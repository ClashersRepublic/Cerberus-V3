using System;
using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Process(Level level)
        {
            Client.LastKeepAlive = DateTime.Now;
            Client.NextKeepAlive = Client.LastKeepAlive.AddSeconds(30);

            Client._keepAliveOk.Send();
        }
    }
}