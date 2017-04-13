﻿using System;
using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.PacketProcessing.Commands.Server
{
    internal class AllianceSettingChangedCommand : Command
    {
        private Alliance m_vAlliance;
        private Level m_vPlayer;

        public AllianceSettingChangedCommand()
        {
        }

        public override byte[] Encode()
        {
            m_vPlayer.Tick();
            List<byte> data = new List<byte>();
            data.AddInt64(m_vAlliance.GetAllianceId());
            data.AddInt32(m_vAlliance.GetAllianceBadgeData());
            data.AddInt32(m_vAlliance.GetAllianceLevel());
            data.AddInt32(0);
            return data.ToArray();
        }

        public void SetAlliance(Alliance alliance)
        {
            m_vAlliance = alliance;
        }

        public void SetPlayer(Level player)
        {
            m_vPlayer = player;
        }
    }
}