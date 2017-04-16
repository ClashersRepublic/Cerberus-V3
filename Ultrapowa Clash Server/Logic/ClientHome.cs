using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class ClientHome : Base
    {
        private const string EventsJson = "{\"events\":[]}";

        private readonly long m_vId;
        private int m_vRemainingShieldTime;
        private string m_vVillage;

        public ClientHome(long userId)
        {
            m_vId = userId;
        }

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt64(m_vId);
            data.AddInt32(m_vRemainingShieldTime);
            data.AddInt32(1800);
            data.AddInt32(0);
            data.Add(1);
            data.AddCompressedString(m_vVillage);
            data.Add(1);
            data.AddCompressedString(EventsJson);

            return data.ToArray();
        }

        public void SetHomeJson(string json)
        {
            m_vVillage = json;
        }

        public void SetShieldTime(int seconds)
        {
            m_vRemainingShieldTime = seconds;
        }
    }
}
