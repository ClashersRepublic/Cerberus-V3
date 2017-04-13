﻿using System;
using UCS.Core;
using UCS.Core.Network;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing
{
    internal class GameOpCommand
    {
        byte m_vRequiredAccountPrivileges;

        public static void SendCommandFailedMessage(Client c)
        {
            Console.WriteLine("GameOp command failed. Insufficient privileges. Requster ID -> " + c.GetLevel().GetPlayerAvatar().GetId());
            var p = new GlobalChatLineMessage(c);
            p.SetChatMessage("GameOp command failed. Insufficient privileges.");
            p.SetPlayerId(0);
            p.SetLeagueId(22);
            p.SetPlayerName("Clash of Magic");
            p.Send();
        }

        public virtual void Execute(Level level)
        {
        }

        public byte GetRequiredAccountPrivileges() => m_vRequiredAccountPrivileges;

        public void SetRequiredAccountPrivileges(byte level)
        {
            m_vRequiredAccountPrivileges = level;
        }
    }
}