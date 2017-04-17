using System.IO;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AllianceInviteMessage : Message
    {
        public AllianceInviteMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
        }
    }
}