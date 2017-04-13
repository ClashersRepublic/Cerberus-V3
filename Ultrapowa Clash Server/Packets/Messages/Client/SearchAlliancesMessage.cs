using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class SearchAlliancesMessage : Message
    {
        public SearchAlliancesMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        private const int m_vAllianceLimit = 40;
        private int m_vAllianceOrigin;
        private int m_vAllianceScore;
        private int m_vMaximumAllianceMembers;
        private int m_vMinimumAllianceLevel;
        private int m_vMinimumAllianceMembers;
        private string m_vSearchString;
        private byte m_vShowOnlyJoinableAlliances;
        private int m_vWarFrequency;

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                m_vWarFrequency = br.ReadInt32();
                m_vAllianceOrigin = br.ReadInt32();
                m_vMinimumAllianceMembers = br.ReadInt32();
                m_vMaximumAllianceMembers = br.ReadInt32();
                m_vAllianceScore = br.ReadInt32();
                m_vShowOnlyJoinableAlliances = br.ReadByte();
                br.ReadInt32();
                m_vMinimumAllianceLevel = br.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            var alliances = ObjectManager.GetInMemoryAlliances();

            if (ResourcesManager.GetInMemoryAlliances().Count == 0)
                 alliances = DatabaseManager.Single().GetAllAlliances();

            var joinableAlliances = new List<Alliance>();
            var i = 0;
            var j = 0;
            while (j < m_vAllianceLimit && i < alliances.Count)
            {
                if (alliances[i].GetAllianceMembers().Count != 0)
                {
                    if (alliances[i].GetAllianceName().Contains(m_vSearchString, StringComparison.OrdinalIgnoreCase))
                    {
                        joinableAlliances.Add(alliances[i]);
                        j++;
                    }
                    i++;
                }
            }
            joinableAlliances = joinableAlliances.ToList();

            var p = new AllianceListMessage(Client);
            p.SetAlliances(joinableAlliances);
            p.SetSearchString(m_vSearchString);
            p.Send();
        }
    }
}