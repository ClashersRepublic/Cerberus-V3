using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class ReplayRequestMessage : Message
    {
        public ReplayRequestMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Process(Level level)
        {
            //new ReplayData(Client).Send();
            new OwnHomeDataMessage(Client, Client.Level).Send();
        }
    }
}