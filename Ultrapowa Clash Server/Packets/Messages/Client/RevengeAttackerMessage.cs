using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    internal class RevengeAttackerMessage : Message
    {
        public RevengeAttackerMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            // 5 int32, no need reading(for now).
        }

        public override void Process(Level level)
        {
            new OwnHomeDataMessage(Client, Client.GetLevel()).Send();
        }
    }
}
