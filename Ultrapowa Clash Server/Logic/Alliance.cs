using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic.StreamEntry;

namespace UCS.Logic
{
    internal class Alliance
    {
        private const int m_vMaxAllianceMembers = 50;
        private const int m_vMaxChatMessagesNumber = 30;
        private readonly Dictionary<long, AllianceMemberEntry> m_vAllianceMembers;
        private readonly System.Collections.Generic.List<UCS.Logic.StreamEntry.StreamEntry> m_vChatMessages;
        private int m_vAllianceBadgeData;
        private string m_vAllianceDescription;
        private int m_vAllianceExperience;
        private long m_vAllianceId;
        private int m_vAllianceLevel;
        private string m_vAllianceName;
        private int m_vAllianceOrigin;
        private int m_vAllianceType;
        private int m_vDrawWars;
        private int m_vLostWars;
        private int m_vRequiredScore;
        private int m_vScore;
        private int m_vWarFrequency;
        private byte m_vWarLogPublic;
        private int m_vWonWars;
        private byte m_vFriendlyWar;

        public Alliance()
        {
            this.m_vChatMessages = new System.Collections.Generic.List<UCS.Logic.StreamEntry.StreamEntry>();
            this.m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public Alliance(long id)
        {
            Random random = new Random();
            this.m_vAllianceId = id;
            this.m_vAllianceName = "Default";
            this.m_vAllianceDescription = "Default";
            this.m_vAllianceBadgeData = 0;
            this.m_vAllianceType = 0;
            this.m_vRequiredScore = 0;
            this.m_vWarFrequency = 0;
            this.m_vAllianceOrigin = 32000001;
            this.m_vScore = 0;
            this.m_vAllianceExperience = random.Next(100, 5000);
            this.m_vAllianceLevel = random.Next(6, 10);
            this.m_vWonWars = random.Next(200, 500);
            this.m_vLostWars = random.Next(100, 300);
            this.m_vDrawWars = random.Next(100, 800);
            this.m_vChatMessages = new System.Collections.Generic.List<UCS.Logic.StreamEntry.StreamEntry>();
            this.m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public void AddAllianceMember(AllianceMemberEntry entry)
        {
            this.m_vAllianceMembers.Add(entry.GetAvatarId(), entry);
        }

        public void AddChatMessage(UCS.Logic.StreamEntry.StreamEntry message)
        {
            if (this.m_vChatMessages.Count >= 30)
                this.m_vChatMessages.RemoveAt(0);
            this.m_vChatMessages.Add(message);
        }

        public byte[] EncodeFullEntry()
        {
            var data = new List<byte>();
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.AddInt32(m_vAllianceType);
            data.AddInt32(m_vAllianceMembers.Count);
            data.AddInt32(m_vScore);
            data.AddInt32(m_vRequiredScore);
            data.AddInt32(m_vWonWars);
            data.AddInt32(m_vLostWars);
            data.AddInt32(m_vDrawWars);
            data.AddInt32(20000001);
            data.AddInt32(m_vWarFrequency);
            data.AddInt32(m_vAllianceOrigin);
            data.AddInt32(m_vAllianceExperience);
            data.AddInt32(m_vAllianceLevel);

            data.AddInt32(0);
            data.AddInt32(0);
            data.Add(m_vWarLogPublic);
            data.Add(m_vFriendlyWar);
            return data.ToArray();
        }

        public byte[] EncodeHeader()
        {
            List<byte> data = new List<byte>();
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.Add(0);
            data.AddInt32(m_vAllianceLevel);
            data.AddInt32(1);
            data.AddInt32(-1);
            return data.ToArray();
        }

        public int GetAllianceBadgeData()
        {
            return this.m_vAllianceBadgeData;
        }


        public string GetAllianceDescription()
        {
            return this.m_vAllianceDescription;
        }

        public int GetAllianceExperience()
        {
            return this.m_vAllianceExperience;
        }

        public long GetAllianceId()
        {
            return this.m_vAllianceId;
        }

        public int GetAllianceLevel()
        {
            return this.m_vAllianceLevel;
        }

        public AllianceMemberEntry GetAllianceMember(long avatarId)
        {
            return this.m_vAllianceMembers[avatarId];
        }

        public System.Collections.Generic.List<AllianceMemberEntry> GetAllianceMembers()
        {
            return this.m_vAllianceMembers.Values.ToList<AllianceMemberEntry>();
        }

        public string GetAllianceName()
        {
            return this.m_vAllianceName;
        }

        public int GetAllianceOrigin()
        {
            return this.m_vAllianceOrigin;
        }

        public int GetAllianceType()
        {
            return this.m_vAllianceType;
        }

        public System.Collections.Generic.List<UCS.Logic.StreamEntry.StreamEntry> GetChatMessages()
        {
            return this.m_vChatMessages;
        }

        public int GetRequiredScore()
        {
            return this.m_vRequiredScore;
        }

        public int GetScore()
        {
            return this.m_vScore;
        }

        public int GetWarFrequency()
        {
            return this.m_vWarFrequency;
        }

        public int GetWarScore()
        {
            return this.m_vWonWars;
        }

        public byte GetWarLogPublic()
        {
            return this.m_vWarLogPublic;
        }

        public byte GetFriendlyWar()
        {
            return this.m_vFriendlyWar;
        }

        public bool IsAllianceFull()
        {
            return this.m_vAllianceMembers.Count >= 50;
        }

        public void LoadFromJSON(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            m_vAllianceId = jsonObject["alliance_id"].ToObject<long>();
            m_vAllianceName = jsonObject["alliance_name"].ToObject<string>();
            m_vAllianceBadgeData = jsonObject["alliance_badge"].ToObject<int>();
            m_vAllianceType = jsonObject["alliance_type"].ToObject<int>();
            m_vRequiredScore = jsonObject["required_score"].ToObject<int>();
            m_vAllianceDescription = jsonObject["description"].ToObject<string>();
            m_vAllianceExperience = jsonObject["alliance_experience"].ToObject<int>();
            m_vAllianceLevel = jsonObject["alliance_level"].ToObject<int>();
            m_vWarLogPublic = jsonObject["war_log_public"].ToObject<byte>();
            m_vFriendlyWar = jsonObject["friendly_war"].ToObject<byte>();
            m_vWonWars = jsonObject["won_wars"].ToObject<int>();
            m_vLostWars = jsonObject["lost_wars"].ToObject<int>();
            m_vDrawWars = jsonObject["draw_wars"].ToObject<int>();
            m_vWarFrequency = jsonObject["war_frequency"].ToObject<int>();
            m_vAllianceOrigin = jsonObject["alliance_origin"].ToObject<int>();
            var jsonMembers = (JArray)jsonObject["members"];
            foreach (JToken jToken in jsonMembers)
            {
                var jsonMember = (JObject)jToken;
                long id = jsonMember["avatar_id"].ToObject<long>();
                var pl = ResourcesManager.GetPlayer(id);
                var member = new AllianceMemberEntry(id);
                m_vScore = m_vScore + pl.GetPlayerAvatar().GetScore();
                member.Load(jsonMember);
                m_vAllianceMembers.Add(id, member);
            }
            m_vScore = m_vScore / 2;
            var jsonMessages = (JArray)jsonObject["chatMessages"];
            if (jsonMessages != null)
            {
                foreach (JToken jToken in jsonMessages)
                {
                    JObject jsonMessage = (JObject)jToken;
                    StreamEntry.StreamEntry se = new StreamEntry.StreamEntry();
                    if (jsonMessage["type"].ToObject<int>() == 1)
                        se = new TroopRequestStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 2)
                        se = new ChatStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 3)
                        se = new InvitationStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 4)
                        se = new AllianceEventStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 5)
                        se = new ShareStreamEntry();
                    se.Load(jsonMessage);
                    m_vChatMessages.Add(se);
                }
            }
        }

        public void RemoveMember(long avatarId) => m_vAllianceMembers.Remove(avatarId);

        public string SaveToJSON()
        {
            var jsonData = new JObject();
            jsonData.Add("alliance_id", m_vAllianceId);
            jsonData.Add("alliance_name", m_vAllianceName);
            jsonData.Add("alliance_badge", m_vAllianceBadgeData);
            jsonData.Add("alliance_type", m_vAllianceType);
            jsonData.Add("score", m_vScore);
            jsonData.Add("required_score", m_vRequiredScore);
            jsonData.Add("description", m_vAllianceDescription);
            jsonData.Add("alliance_experience", m_vAllianceExperience);
            jsonData.Add("alliance_level", m_vAllianceLevel);
            jsonData.Add("war_log_public", m_vWarLogPublic);
            jsonData.Add("friendly_war", m_vFriendlyWar);
            jsonData.Add("won_wars", m_vWonWars);
            jsonData.Add("lost_wars", m_vLostWars);
            jsonData.Add("draw_wars", m_vDrawWars);
            jsonData.Add("war_frequency", m_vWarFrequency);
            jsonData.Add("alliance_origin", m_vAllianceOrigin);
            var jsonMembersArray = new JArray();
            foreach (AllianceMemberEntry member in m_vAllianceMembers.Values)
            {
                var jsonObject = new JObject();
                member.Save(jsonObject);
                jsonMembersArray.Add(jsonObject);
            }
            jsonData.Add("members", jsonMembersArray);
            var jsonMessageArray = new JArray();
            foreach (StreamEntry.StreamEntry message in m_vChatMessages)
            {
                var jsonObject = new JObject();
                message.Save(jsonObject);
                jsonMessageArray.Add(jsonObject);
            }
            jsonData.Add("chatMessages", jsonMessageArray);
            return JsonConvert.SerializeObject(jsonData);
        }

        public void SetAllianceBadgeData(int data)
        {
            this.m_vAllianceBadgeData = data;
        }

        public void SetAllianceDescription(string description)
        {
            this.m_vAllianceDescription = description;
        }

        public void SetAllianceLevel(int level)
        {
            this.m_vAllianceLevel = level;
        }

        public void SetAllianceName(string name)
        {
            this.m_vAllianceName = name;
        }

        public void SetAllianceOrigin(int origin)
        {
            this.m_vAllianceOrigin = origin;
        }

        public void SetAllianceType(int status)
        {
            this.m_vAllianceType = status;
        }

        public void SetRequiredScore(int score)
        {
            this.m_vRequiredScore = score;
        }

        public void SetWarFrequency(int frequency)
        {
            this.m_vWarFrequency = frequency;
        }

        public void SetWarPublicStatus(byte log)
        {
            this.m_vWarLogPublic = log;
        }

        public void SetFriendlyWar(byte log)
        {
            this.m_vFriendlyWar = log;
        }

        public void SetWarAndFriendlytStatus(byte status)
        {
            if ((int)status == 1)
                this.SetWarPublicStatus((byte)1);
            else if ((int)status == 2)
                this.SetFriendlyWar((byte)1);
            else if ((int)status == 3)
            {
                this.SetWarPublicStatus((byte)1);
                this.SetFriendlyWar((byte)1);
            }
            else
            {
                if ((int)status != 0)
                    return;
                this.SetWarPublicStatus((byte)0);
                this.SetFriendlyWar((byte)0);
            }
        }
    }
}
