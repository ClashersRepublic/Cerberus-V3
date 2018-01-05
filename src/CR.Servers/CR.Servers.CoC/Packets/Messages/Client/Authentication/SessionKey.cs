using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Stream;
using CR.Servers.CoC.Packets.Stream.Scrambler;
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
            string scrambledNonce = null;
            RC4Scrambler rc4Scrambler = new RC4Scrambler(this.Device.EncryptionSeed);

            byte byte100 = 0;

            for (int i = 0; i < 100; i++)
            {
                byte100 = (byte) rc4Scrambler.NextInt();
            }

            for (int i = 0; i < this.Nonce.Length; i++)
            {
                scrambledNonce += (char) (this.Nonce[i] ^ ((byte) rc4Scrambler.NextInt() & byte100));
            }

            this.Device.InitRC4Encrypters(Factory.RC4Key, scrambledNonce);
        }
    }
}