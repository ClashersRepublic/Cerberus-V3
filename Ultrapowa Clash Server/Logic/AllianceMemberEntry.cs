using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UCS.Core;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class AllianceMemberEntry
    {
        private readonly int[] m_vRoleTable = new int[5]
        {
            1,
            1,
            4,
            2,
            3
        };

        private readonly int m_vDonatedTroops;
        private readonly byte m_vIsNewMember;
        private readonly int m_vReceivedTroops;
        private readonly int m_vWarCooldown;
        private int m_vWarOptInStatus;
        private long m_vAvatarId;
        private int m_vOrder;
        private int m_vPreviousOrder;
        private int m_vRole;

        public AllianceMemberEntry(long avatarId)
        {
            this.m_vAvatarId = avatarId;
            this.m_vIsNewMember = (byte)0;
            this.m_vOrder = 1;
            this.m_vPreviousOrder = 1;
            this.m_vRole = 1;
            this.m_vDonatedTroops = 200;
            this.m_vReceivedTroops = 100;
            this.m_vWarCooldown = 0;
            this.m_vWarOptInStatus = 1;
        }

        public static int GetDonations() => 150;

        public byte[] Encode()
        {
            var data = new List<byte>();
            var avatar = ResourcesManager.GetPlayer(m_vAvatarId);
            data.AddInt64(m_vAvatarId);
            data.AddString(avatar.GetPlayerAvatar().GetAvatarName());
            data.AddInt32(m_vRole);
            data.AddInt32(avatar.GetPlayerAvatar().GetAvatarLevel());
            data.AddInt32(avatar.GetPlayerAvatar().GetLeagueId());
            data.AddInt32(avatar.GetPlayerAvatar().GetScore());
            data.AddInt32(m_vDonatedTroops);
            data.AddInt32(m_vReceivedTroops);
            data.AddInt32(m_vOrder);
            data.AddInt32(m_vPreviousOrder);
            data.AddInt32(m_vIsNewMember);
            data.AddInt32(m_vWarCooldown);
            data.AddInt32(m_vWarOptInStatus);
            data.Add(1);
            data.AddInt64(m_vAvatarId);
            return data.ToArray();
        }

        public long GetAvatarId() => m_vAvatarId;
        public int GetOrder() => m_vOrder;
        public int GetPreviousOrder() => m_vPreviousOrder;
        public int GetRole() => m_vRole;

        public bool HasLowerRoleThan(int role)
        {
            var result = true;
            if (role < m_vRoleTable.Length && m_vRole < m_vRoleTable.Length)
            {
                if (m_vRoleTable[m_vRole] >= m_vRoleTable[role])
                    result = false;
            }
            return result;
        }

        public byte IsNewMember() => m_vIsNewMember;

        public void Load(JObject jsonObject)
        {
            m_vAvatarId = jsonObject["avatar_id"].ToObject<long>();
            m_vRole = jsonObject["role"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("avatar_id", m_vAvatarId);
            jsonObject.Add("role", m_vRole);
            return jsonObject;
        }

        public void SetAvatarId(long id)
        {
            m_vAvatarId = id;
        }

        public void SetOrder(int order)
        {
            m_vOrder = order;
        }

        public void SetPreviousOrder(int order)
        {
            m_vPreviousOrder = order;
        }

        public void SetRole(int role)
        {
            m_vRole = role;
        }
    }
}
