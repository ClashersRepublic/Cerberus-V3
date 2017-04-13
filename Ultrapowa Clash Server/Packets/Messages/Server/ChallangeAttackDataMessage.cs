using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.Packets.Messages.Server
{
    internal class ChallangeAttackDataMessage : Message
    {
        internal readonly Level m_vOwnerLevel;
        internal readonly Level m_vVisitorLevel;

        public ChallangeAttackDataMessage(PacketProcessing.Client client, Level level) : base(client)
        {
            SetMessageType(24107);
            m_vOwnerLevel = level;
            m_vVisitorLevel = client.GetLevel();
            level.GetPlayerAvatar().State = ClientAvatar.UserState.CHA;
        }

        public override void Encode()
        {
            ChallangeAttackDataMessage attackDataMessage = this;
            try
            {
                var data = new List<byte>();
                data.AddRange((IEnumerable<byte>)new byte[12]
            {
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 240,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          (byte) 84,
          (byte) 206,
          (byte) 92,
          (byte) 74
            });
                var ch = new ClientHome(m_vOwnerLevel.GetPlayerAvatar().GetId());
                ch.SetHomeJSON(m_vOwnerLevel.SaveToJSON());
                data.AddRange((IEnumerable<byte>)ch.Encode());
                data.AddRange((IEnumerable<byte>)m_vOwnerLevel.GetPlayerAvatar().Encode());
                data.AddRange((IEnumerable<byte>)m_vVisitorLevel.GetPlayerAvatar().Encode());
                data.AddRange((IEnumerable<byte>)new byte[5]
                {
                0x00, 0x00, 0x00, 0x03, 0x00
                });
                data.AddInt32(0);
                data.AddInt32(0);
                data.AddInt64(0);
                data.AddInt64(0);
                data.AddInt64(0);
                Encrypt(data.ToArray());
            }
            catch (Exception ex)
            {
            }
        }
    }
}