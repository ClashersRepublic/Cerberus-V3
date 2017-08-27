using System.Collections.Generic;
using System.Linq;
using Magic.Files;
using Magic.Royale.Core.Crypto;
using Magic.Royale.Core.Settings;
using Magic.Royale.Extensions.Blake2B;
using Magic.Royale.Extensions.List;
using Magic.Royale.Extensions.Sodium;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Network.Messages.Server.Authentication
{
    internal class Authentication_Failed : Message
    {
        public Authentication_Failed(Device Device, Reason Reason = Reason.Default) : base(Device)
        {
            Identifier = 20103;
            this.Reason = Reason;
        }


        internal Reason Reason = Reason.Default;

        internal string PatchingHost => Fingerprint.Custom
            ? Constants.PatchServer
            : "https://www.clashersrepublic.com/game-content/projectmagic/";

        internal string Message;
        internal string RedirectDomain;

        public override void Encode()
        {
            Data.AddVInt((int) Reason);
            Data.AddString(Reason == Reason.Patch ? Fingerprint.Json : null);
            Data.AddString(RedirectDomain);
            Data.AddString(PatchingHost);
            Data.AddString(Constants.UpdateServer);
            Data.AddString(Message);
            Data.AddVInt(Reason == Reason.Maintenance ? 0 : 1);
            Data.AddByte(0);
            Data.AddInt(-1);
        }

        public override void Encrypt()
        {
            if (Device.State >= State.LOGIN)
            {
                var Blake = new Blake2BHasher();

                Blake.Update(Device.SNonce);
                Blake.Update(Device.PublicKey);
                Blake.Update(Key.PublicKey);

                var Nonce = Blake.Finish();
                var Encrypted = Device.RNonce.Concat(Device.PublicKey).Concat(Data).ToArray();

                Data = new List<byte>(Sodium.Encrypt(Encrypted, Nonce, Key.Crypto.PrivateKey, Device.PublicKey));
            }

            Length = (ushort) Data.Count;
        }
    }
}
