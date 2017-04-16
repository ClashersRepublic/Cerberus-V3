using System;
using System.Collections.Generic;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class EnemyHomeDataMessage : Message
    {
        internal readonly Level DefenderLevel;
        internal readonly Level AttackerLevel;

        public EnemyHomeDataMessage(PacketProcessing.Client client, Level defenderLevel, Level attackerLevel) : base(client)
        {
            SetMessageType(24107);

            DefenderLevel = defenderLevel;
            AttackerLevel = attackerLevel;
        }

        public override void Encode()
        {
            try
            {
                AttackerLevel.GetPlayerAvatar().State = ClientAvatar.UserState.PVP;

                var data = new List<byte>();
                var home = new ClientHome(DefenderLevel.GetPlayerAvatar().GetId());
                home.SetShieldTime(DefenderLevel.GetPlayerAvatar().RemainingShieldTime);
                home.SetHomeJson(DefenderLevel.SaveToJson());

                data.AddInt32((int)TimeSpan.FromSeconds(100).TotalSeconds);
                data.AddInt32(-1);
                data.AddInt32((int)Client.GetLevel().GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                data.AddRange(home.Encode());
                data.AddRange(DefenderLevel.GetPlayerAvatar().Encode());
                data.AddRange(AttackerLevel.GetPlayerAvatar().Encode());
                data.AddInt32(3);
                data.AddInt32(0);
                data.Add(0);

                Encrypt(data.ToArray());
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Unable to encode EnemyHomeDataMessage, returning home.");
                new OwnHomeDataMessage(AttackerLevel.GetClient(), AttackerLevel).Send();
            }
        }
    }
}