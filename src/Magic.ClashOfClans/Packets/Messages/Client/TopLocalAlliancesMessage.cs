using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    // Packet 14402
    internal class TopLocalAlliancesMessage : Message
    {
        public TopLocalAlliancesMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {              
              new LocalAlliancesMessage(Client).Send();
        }
    }
}
