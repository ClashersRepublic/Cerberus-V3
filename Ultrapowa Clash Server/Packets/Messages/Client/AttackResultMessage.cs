using System;
using System.IO;
using System.Text;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Client
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