using System.IO;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
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
            unknown = GetData().Length == 10 ? GetData()[9] : Reader.Read();
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
