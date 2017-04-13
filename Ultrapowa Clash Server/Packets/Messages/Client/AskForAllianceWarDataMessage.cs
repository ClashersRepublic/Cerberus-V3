using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class AskForAllianceWarDataMessage : Message
    {
        public AskForAllianceWarDataMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
            }
        }

        public override void Process(Level level)
        {
            new AllianceWarDataMessage(Client).Send();
        }
    }
}