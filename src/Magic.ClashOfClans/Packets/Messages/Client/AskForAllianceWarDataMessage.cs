using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AskForAllianceWarDataMessage : Message
    {
        public AskForAllianceWarDataMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
            }
        }

        public override void Process(Level level)
        {
            new AllianceWarDataMessage(Client).Send();
        }
    }
}