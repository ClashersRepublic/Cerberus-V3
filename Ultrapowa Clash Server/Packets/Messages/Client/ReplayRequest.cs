using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
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
            new OwnHomeDataMessage(Client, Client.GetLevel()).Send();
        }
    }
}