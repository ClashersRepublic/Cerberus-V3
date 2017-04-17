using System.IO;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    // Packet 14401
    internal class TopGlobalAlliancesMessage : Message
    {
        public int unknown;

        public TopGlobalAlliancesMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            unknown = Data.Length == 10 ? Data[9] : Reader.Read();
        }

        public override void Process(Level level)
        {
            if (unknown == 0)
                new GlobalAlliancesMessage(Client).Send();
            else
                new LocalAlliancesMessage(Client).Send();
        }
    }
}
