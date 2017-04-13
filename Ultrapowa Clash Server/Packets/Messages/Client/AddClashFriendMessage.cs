using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.Packets.Messages.Client
{
    internal class AddClashFriendMessage : Message
    {
        public long FriendID { get; set; }

        public AddClashFriendMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                 FriendID = br.ReadInt64();  
            }
        }

        public override void Process(Level level)
        {
        }
    }
}
