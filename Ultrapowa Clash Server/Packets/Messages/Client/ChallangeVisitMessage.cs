using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    internal class ChallangeVisitMessage : Message
    {
        public long AvatarID { get; set; }

        public ChallangeVisitMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

	public override void Decode()
        {
		using (var r = new PacketReader(new MemoryStream(GetData())))
		{
			AvatarID = r.ReadInt64();
		}
	}

        public override void Process(Level level)
        {
		new OwnHomeDataMessage(Client, level).Send();
        }
    }
}
