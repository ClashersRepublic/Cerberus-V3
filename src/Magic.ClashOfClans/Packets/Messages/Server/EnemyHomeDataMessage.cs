﻿using System;
using System.Collections.Generic;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Messages.Server
{
    internal class EnemyHomeDataMessage : Message
    {
        internal readonly Level DefenderLevel;
        internal readonly Level AttackerLevel;

        public EnemyHomeDataMessage(PacketProcessing.Client client, Level defenderLevel, Level attackerLevel) : base(client)
        {
            MessageType = 24107;

            DefenderLevel = defenderLevel;
            AttackerLevel = attackerLevel;
        }

        public override void Encode()
        {
            try
            {
                AttackerLevel.Avatar.State = ClientAvatar.UserState.PVP;

                var data = new List<byte>();
                var home = new ClientHome(DefenderLevel.Avatar.Id);
                home.SetShieldTime(DefenderLevel.Avatar.RemainingShieldTime);
                home.SetHomeJson(DefenderLevel.SaveToJson());

                data.AddInt32((int)TimeSpan.FromSeconds(100).TotalSeconds);
                data.AddInt32(-1);
                data.AddInt32((int)Client.Level.Time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                data.AddRange(home.Encode());
                data.AddRange(DefenderLevel.Avatar.Encode());
                data.AddRange(AttackerLevel.Avatar.Encode());
                data.AddInt32(3);
                data.AddInt32(0);
                data.Add(0);

                Encrypt(data.ToArray());
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Unable to encode EnemyHomeDataMessage, returning home.");
                new OwnHomeDataMessage(AttackerLevel.Client, AttackerLevel).Send();
            }
        }
    }
}