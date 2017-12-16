using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Cryptography;
using CR.Servers.Extensions.List;
using CR.Servers.Library;

namespace CR.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class SessionKey : Message
    {
        internal override short Type => 20000;

        public byte[] Nonce;

        public SessionKey(Device device) : base(device)
        {
            this.Nonce = new byte[16];
            Resources.Random.NextBytes(this.Nonce);
        }

        internal override void Encode()
        {
            Data.AddByteArray(this.Nonce);
            Data.AddInt(1);
        }

        internal override void Process()
        {
            string Nonce = Rjindael.ScrambleNonce(this.Device.Seed, this.Nonce);

            ((RC4Encrypter)this.Device.ReceiveDecrypter).Init(Factory.RC4Key + Nonce);
            ((RC4Encrypter)this.Device.SendEncrypter).Init(Factory.RC4Key + Nonce);
        }
    }
}