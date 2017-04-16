using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class GlobalChatLineMessage : Message
    {
        internal readonly int m_vPlayerLevel;
        internal int m_vAllianceIcon;
        internal int m_vLeagueId;
        internal long m_vAllianceId;
        internal string m_vAllianceName;
        internal long m_vCurrentHomeId;
        internal bool m_vHasAlliance;
        internal long m_vHomeId;
        internal string m_vMessage;
        internal string m_vPlayerName;

        public GlobalChatLineMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24715);

            m_vMessage = "default";
            m_vPlayerName = "default";
            m_vHomeId = 1;
            m_vCurrentHomeId = 1;
            m_vPlayerLevel = 1;
            m_vHasAlliance = false;
        }

        public override void Encode()
        {
            var pack = new List<byte>();

            pack.AddString(m_vMessage);
            pack.AddString(m_vPlayerName);
            pack.AddInt32(m_vPlayerLevel);
            pack.AddInt32(m_vLeagueId);
            pack.AddInt64(m_vHomeId);
            pack.AddInt64(m_vCurrentHomeId);

            if (!m_vHasAlliance)
            {
                pack.Add(0);
            }
            else
            {
                pack.Add(1);
                pack.AddInt64(m_vAllianceId);
                pack.AddString(m_vAllianceName);
                pack.AddInt32(m_vAllianceIcon);
            }

            Encrypt(pack.ToArray());
        }

        public void SetAlliance(Alliance alliance)
        {
            if (alliance == null || alliance.GetAllianceId() <= 0L)
            {
                // Just in case.
                m_vHasAlliance = false;
                return;
            }

            m_vHasAlliance = true;
            m_vAllianceId = alliance.GetAllianceId();
            m_vAllianceName = alliance.GetAllianceName();
            m_vAllianceIcon = alliance.GetAllianceBadgeData();
        }

        public void SetChatMessage(string message)
        {
            m_vMessage = message;
        }

        public void SetLeagueId(int leagueId)
        {
            m_vLeagueId = leagueId;
        }

        public void SetPlayerId(long id)
        {
            m_vHomeId = id;
            m_vCurrentHomeId = id;
        }

        public void SetPlayerName(string name)
        {
            m_vPlayerName = name;
        }
    }
}