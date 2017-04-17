using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AttackNpcMessage : Message
    {
        public int LevelId { get; set; }

        public AttackNpcMessage(PacketProcessing.Client client, PacketReader br)
            : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                LevelId = br.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            new NpcDataMessage(Client, level, this).Send();
        }
    }
}
