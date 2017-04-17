using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class GlobalAlliancesMessage : Message
    {
        List<Alliance> m_vAlliances;

        public GlobalAlliancesMessage(PacketProcessing.Client client) : base(client)
        {
            MessageType = 24401;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var packet1 = new List<byte>();
            var i = 0;

            foreach (var alliance in ObjectManager.GetInMemoryAlliances().OrderByDescending(t => t.GetScore()))
            {
                if (i < 100)
                {
                    packet1.AddInt64(alliance.GetAllianceId());
                    packet1.AddString(alliance.GetAllianceName());
                    packet1.AddInt32(i + 1);
                    packet1.AddInt32(alliance.GetScore());
                    packet1.AddInt32(i + 1);
                    packet1.AddInt32(alliance.GetAllianceBadgeData());
                    packet1.AddInt32(alliance.GetAllianceMembers().Count);
                    packet1.AddInt32(0);
                    packet1.AddInt32(alliance.GetAllianceLevel());
                    i++;
                }
                else
                    break;
            }

            data.AddInt32(i);
            data.AddRange((IEnumerable<byte>)packet1);

            data.AddInt32((int)TimeSpan.FromDays(1).TotalSeconds);
            data.AddInt32(3);
            data.AddInt32(50000);
            data.AddInt32(30000);
            data.AddInt32(15000);
            Encrypt(data.ToArray());
        }
    }
}
