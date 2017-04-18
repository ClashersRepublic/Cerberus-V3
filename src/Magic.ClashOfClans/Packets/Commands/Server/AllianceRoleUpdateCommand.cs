using System;
using System.Collections.Generic;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;

namespace  Magic.PacketProcessing.Commands.Server
{
    internal class AllianceRoleUpdateCommand : Command
    {
        public Alliance m_vAlliance;

        public int Role { get; set; }

        public AllianceRoleUpdateCommand()
        {
        }

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddInt64(m_vAlliance.AllianceId);
            data.AddInt32(Role);
            data.AddInt32(Role);
            data.AddInt32(0);
            return data.ToArray();
        }

        public void SetAlliance(Alliance a)
        {
            m_vAlliance = a;
        }

        public void SetRole(int role)
        {
            Role = role;
        }

        [Obsolete]
        public void Tick(Level level)
        {
            level.Tick();
        }
    }
}
