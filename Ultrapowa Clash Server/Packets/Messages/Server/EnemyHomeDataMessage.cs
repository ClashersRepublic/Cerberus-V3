using System;
using System.Collections.Generic;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class EnemyHomeDataMessage : Message
    {
        internal readonly Level OwnerLevel;
        internal readonly Level VisitorLevel;

        public EnemyHomeDataMessage(PacketProcessing.Client client, Level ownerLevel, Level visitorLevel) : base(client)
        {
            SetMessageType(24107);
            OwnerLevel = ownerLevel;
            VisitorLevel = visitorLevel;
        }

        public override void Encode()
        {
            EnemyHomeDataMessage enemyHomeDataMessage = this;
            try
            {
                OwnerLevel.GetPlayerAvatar().State = ClientAvatar.UserState.PVP;
                List<byte> data = new List<byte>();
                ClientHome ch = new ClientHome(OwnerLevel.GetPlayerAvatar().GetId());
                ch.SetShieldTime(OwnerLevel.GetPlayerAvatar().RemainingShieldTime);
                ch.SetHomeJSON(OwnerLevel.SaveToJSON());

                data.AddInt32((int)TimeSpan.FromSeconds(100).TotalSeconds);
                data.AddInt32(-1);
                data.AddInt32((int)Client.GetLevel().GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                data.AddRange((IEnumerable<byte>)ch.Encode());
                data.AddRange((IEnumerable<byte>)OwnerLevel.GetPlayerAvatar().Encode());
                data.AddRange((IEnumerable<byte>)VisitorLevel.GetPlayerAvatar().Encode());
                data.AddInt32(3);
                data.AddInt32(0);
                data.Add(0);

                Encrypt(data.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to encode EnemyHomeDataMessage, retunring home. " + ex.ToString());
                new OwnHomeDataMessage(OwnerLevel.GetClient(), OwnerLevel);
            }
        }
    }
}