using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class NewsSeenMessage : Message
    {

        public NewsSeenMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {

        }
    }
}