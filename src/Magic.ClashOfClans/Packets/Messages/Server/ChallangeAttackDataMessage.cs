using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;

namespace Magic.Packets.Messages.Server
{
    internal class ChallangeAttackDataMessage : Message
    {
        internal readonly Level m_vOwnerLevel;
        internal readonly Level m_vVisitorLevel;

        public ChallangeAttackDataMessage(PacketProcessing.Client client, Level level) : base(client)
        {
            MessageType = 24107;

            m_vOwnerLevel = level;
            m_vVisitorLevel = client.Level;
            level.GetPlayerAvatar().State = ClientAvatar.UserState.CHA;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddRange(new byte[12]
            {
                0,
                0,
                0,
                240,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                84,
                206,
                92,
                74
            });

            var home = new ClientHome(m_vOwnerLevel.GetPlayerAvatar().GetId());
            home.SetHomeJson(m_vOwnerLevel.SaveToJson());

            data.AddRange(home.Encode());
            data.AddRange(m_vOwnerLevel.GetPlayerAvatar().Encode());
            data.AddRange(m_vVisitorLevel.GetPlayerAvatar().Encode());

            data.AddRange(new byte[5]
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
    }
}