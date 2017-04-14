using System;
using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {

        }

        public override void Process(Level level)
        {
            Client.LastKeepAlive = DateTime.Now;
            Client.NextKeepAlive = Client.LastKeepAlive.AddSeconds(30);

            Client.m_vKeepAliveOk.Send();
        }
    }
}