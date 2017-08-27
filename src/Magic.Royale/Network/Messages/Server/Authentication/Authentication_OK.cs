using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Royale.Core.Crypto;
using Magic.Royale.Extensions;
using Magic.Royale.Extensions.Blake2B;
using Magic.Royale.Extensions.List;
using Magic.Royale.Extensions.Sodium;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Network.Messages.Server.Authentication
{
    internal class Authentication_OK : Message
    {
        public Authentication_OK(Device client) : base(client)
        {
            Identifier = 20104;
            Device.State = State.LOGGED;
        }


        public override void Encode()
        {
            Data.AddLong(Device.Player.UserId); // UserID

            Data.AddLong(Device.Player.UserId); // HomeID

            Data.AddString(Device.Player.Token); // Token


            Data.AddString(null); // Facebook

            Data.AddString(null); // Gamecenter

            Data.AddVInt(Device.Major); // Major
            Data.AddVInt(Device.Minor); // Minor
            Data.AddVInt(Device.Revision); // Revision

            Data.AddString("prod");

            Data.AddVInt(0); // Session Count
            Data.AddVInt(0); // Total Play Time Seconds
            Data.AddVInt(0); // Time since creation

            Data.AddString("1609113955765603"); // Facebook ID

            Data.AddString(TimeUtils.ToJavaTimestamp(DateTime.Now).ToString()); // Server Time
            Data.AddString(TimeUtils.ToJavaTimestamp(Device.Player.Created).ToString()); // Account Creation Date

            Data.AddVInt(0); // VInt


            Data.AddString(null); // Google Service ID

            Data.AddString(null);
            Data.AddString(null);

            Data.AddString(null); // Region

            Data.AddString(
                "http://7166046b142482e67b30-2a63f4436c967aa7d355061bd0d924a1.r65.cf1.rackcdn.com"); // Content URL
            Data.AddString("https://event-assets.clashroyale.com"); // Event Asset URL

            Data.AddByte(1);
        }

        public override void Encrypt()
        {
            var Blake = new Blake2BHasher();

            Blake.Update(Device.SNonce);
            Blake.Update(Device.PublicKey);
            Blake.Update(Key.PublicKey);

            var Nonce = Blake.Finish();
            var Encrypted = Device.RNonce.Concat(Device.PublicKey).Concat(Data)
                .ToArray();

            Data = new List<byte>(Sodium.Encrypt(Encrypted, Nonce, Key.PrivateKey, Device.PublicKey));

            Length = (ushort) Data.Count;
        }
    }
}
