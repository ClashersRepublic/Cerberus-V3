using CR.Servers.CoC.Extensions;

namespace CR.Servers.CoC.Packets.Cryptography
{
    public class RC4Encrypter : IEncrypter
    {
        public bool IsRC4 => true;

        private byte i;
        private byte j;
        private byte[] Key;

        public RC4Encrypter(string Key)
        {
            this.Init(Key);
        }

        public RC4Encrypter(string Key, string Nonce)
        {
            this.Init(Key + Nonce);
        }

        internal void Init(string Key)
        {
            this.i = 0;
            this.j = 0;

            this.Key = this.KSA(Extension.StringToByteArray(Key));

            for (int k = 0; k < Key.Length; k++)
            {
                this.PRGA();
            }
        }

        internal byte PRGA()
        {
            this.i = (byte)(this.i + 1);
            this.j = (byte)(this.j + this.Key[this.i]);

            byte SwapTemp = this.Key[this.i];

            this.Key[this.i] = this.Key[this.j];
            this.Key[this.j] = SwapTemp;

            return this.Key[(this.Key[this.i] + this.Key[this.j]) % 256];
        }

        private byte[] KSA(byte[] key)
        {
            var keyLength = key.Length;
            var S = new byte[256];

            for (int i = 0; i != 256; i++)
            {
                S[i] = (byte)i;
            }

            byte j = 0;

            for (int i = 0; i != 256; i++)
            {
                j = (byte)((j + S[i] + key[i % keyLength]) % 256); 

                var SwapTemp = S[i];
                S[i] = S[j];
                S[j] = SwapTemp;
            }

            return S;
        }

        public byte[] Decrypt(byte[] Data)
        {
            int Length = Data.Length;

            if (Length > 0)
            {
                for (int i = 0; i < Length; i++)
                {
                    Data[i] ^= this.PRGA();
                }
            }

            return Data;
        }
        
        public byte[] Encrypt(byte[] Data)
        {
            int Length = Data.Length;

            if (Length > 0)
            {
                for (int i = 0; i < Length; i++)
                {
                    Data[i] ^= this.PRGA();
                }
            }

            return Data;
        }
    }
}
