using System;
using System.Collections.Generic;
using System.Linq;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class VisitedHomeDataMessage : Message
    {
        private readonly Level m_vOwnerLevel;
        private readonly Level m_vVisitorLevel;

        public VisitedHomeDataMessage(PacketProcessing.Client client, Level ownerLevel, Level visitorLevel) : base(client)
        {
            SetMessageType(24113);
            m_vOwnerLevel = ownerLevel;
            m_vVisitorLevel = visitorLevel;
        }

        public override void Encode()
        {
            VisitedHomeDataMessage visitedHomeDataMessage = this;
            try
            {
                List<byte> data = new List<byte>();
                ClientHome clientHome = new ClientHome(m_vOwnerLevel.GetPlayerAvatar().GetId());
                clientHome.SetShieldTime(m_vOwnerLevel.GetPlayerAvatar().RemainingShieldTime);
                clientHome.SetHomeJson(m_vOwnerLevel.SaveToJson());
                data.AddInt32(-1);
                data.AddInt32((int)TimeSpan.FromSeconds(100).TotalSeconds);
                data.AddRange((IEnumerable<byte>)clientHome.Encode());
                data.AddRange((IEnumerable<byte>)m_vOwnerLevel.GetPlayerAvatar().Encode());
                data.AddInt32(0);
                data.Add((byte)1);
                data.AddRange(m_vVisitorLevel.GetPlayerAvatar().Encode());
                Encrypt(data.ToArray());
            }
            catch (Exception ex)
            {
            }
        }
    }
}
