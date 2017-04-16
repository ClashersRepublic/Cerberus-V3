﻿using System;
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
        public SearchAlliancesMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
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
        private int m_vTrophyLimit;

        public override void Decode()
        {
            using (var reader = new PacketReader(new MemoryStream(GetData())))
            {
                m_vSearchString = reader.ReadString();
                m_vWarFrequency = reader.ReadInt32();
                m_vAllianceOrigin = reader.ReadInt32();
                m_vMinimumAllianceMembers = reader.ReadInt32();
                m_vMaximumAllianceMembers = reader.ReadInt32();
                m_vAllianceScore = reader.ReadInt32();
                m_vTrophyLimit = reader.ReadInt32();
                m_vShowOnlyJoinableAlliances = reader.ReadByte();

                reader.ReadInt32();

                m_vMinimumAllianceLevel = reader.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            var alliances = ObjectManager.GetInMemoryAlliances();

            if (ResourcesManager.GetInMemoryAlliances().Count == 0)
                alliances = DatabaseManager.Instance.GetAllAlliances();

            var joinableAlliances = new List<Alliance>();
            for (int i = 0; i < alliances.Count; i++)
            {
                if (joinableAlliances.Count >= m_vAllianceLimit)
                    break;

                var alliance = alliances[i];
                if (m_vSearchString == null)
                {
                    joinableAlliances.Add(alliance);
                }
                else
                {
                    if (alliance.GetAllianceName().Contains(m_vSearchString, StringComparison.OrdinalIgnoreCase))
                    {
                        joinableAlliances.Add(alliance);
                    }
                }
            }

            joinableAlliances = joinableAlliances.ToList();

            var message = new AllianceListMessage(Client);
            message.SetAlliances(joinableAlliances);
            message.SetSearchString(m_vSearchString);
            message.Send();
        }
    }
}