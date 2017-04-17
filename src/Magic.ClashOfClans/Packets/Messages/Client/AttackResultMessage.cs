using System;
using System.IO;
using System.Text;
using Magic.Core;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AttackResultMessage : Message
    {
        public AttackResultMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
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