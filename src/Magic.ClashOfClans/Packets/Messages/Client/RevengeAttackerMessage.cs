using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.Packets.Messages.Client
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
            new OwnHomeDataMessage(Client, Client.Level).Send();
        }
    }
}
