using System.Collections.Generic;
using System.IO;
using System.Linq;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class AskForJoinableAlliancesListMessage : Message
    {
        private const int m_vAllianceLimit = 40;

        public AskForJoinableAlliancesListMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            List<Alliance> inMemoryAlliances = ObjectManager.GetInMemoryAlliances();
            List<Alliance> source = new List<Alliance>();
            int index1 = 0;
            for (int index2 = 0; index2 < 40 && index1 < inMemoryAlliances.Count; ++index1)
            {
                if (inMemoryAlliances[index1].GetAllianceMembers().Count != 0 && !inMemoryAlliances[index1].IsAllianceFull())
                {
                    source.Add(inMemoryAlliances[index1]);
                    ++index2;
                }
            }
            List<Alliance> list = source.ToList<Alliance>();
            new JoinableAllianceListMessage(Client)
            {
                m_vAlliances = list
            }.Send();
        }
    }
}
