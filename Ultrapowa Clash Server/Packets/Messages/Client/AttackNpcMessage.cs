using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
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
            using (var br = new PacketReader(new MemoryStream(GetData())))
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
