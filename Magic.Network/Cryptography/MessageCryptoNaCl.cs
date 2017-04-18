using Magic.Network.Cryptography.NaCl;
using Magic.Network.Cryptography.NaCl.Internal.Blake2B;
using System;

namespace Magic.Network.Cryptography
{
    public partial class MessageCryptoNaCl
    {
        static MessageCryptoNaCl()
        {
            //TODO: Set some days when we run our own proxy.
            _scPk = null;
        }

        private static readonly byte[] _standardPrivateKey = new byte[]
        {
            0x18, 0x91, 0xD4, 0x01, 0xFA, 0xDB, 0x51, 0xD2, 0x5D, 0x3A, 0x91, 0x74,
            0xD4, 0x72, 0xA9, 0xF6, 0x91, 0xA4, 0x5B, 0x97, 0x42, 0x85, 0xD4, 0x77,
            0x29, 0xC4, 0x5C, 0x65, 0x38, 0x07, 0x0D, 0x85
        };

        private static readonly byte[] _standardPublicKey = new byte[] // == PublicKeyBox.GenerateKeyPair(_standardPrivateKey);
        {
            0x72, 0xF1, 0xA4, 0xA4, 0xC4, 0x8E, 0x44, 0xDA, 0x0C, 0x42, 0x31, 0x0F,
            0x80, 0x0E, 0x96, 0x62, 0x4E, 0x6D, 0xC6, 0xA6, 0x41, 0xA9, 0xD4, 0x1C,
            0x3B, 0x50, 0x39, 0xD8, 0xDF, 0xAD, 0xC2, 0x7E
        };

        private static readonly byte[] _scPk;
        public static byte[] SupercellPublicKey => _scPk;

        public static KeyPair StandardKeyPair
        {
            // Cloning just not to mess up with refs
            get { return new KeyPair((byte[])_standardPublicKey.Clone(), (byte[])_standardPrivateKey.Clone()); }
        }

        // Generate blake2b nonce with clientkey(pk) and serverkey.
        private static byte[] GenerateBlake2BNonce(byte[] clientKey, byte[] serverKey)
        {
            var hashBuffer = new byte[clientKey.Length + serverKey.Length];

            Buffer.BlockCopy(clientKey, 0, hashBuffer, 0, clientKey.Length);
            Buffer.BlockCopy(serverKey, 0, hashBuffer, PublicKeyBox.PublicKeyLength, serverKey.Length);

            using (var blake = new Blake2B(24))
                return blake.ComputeHash(hashBuffer);
        }

        // Generate blake2b nonce with snonce, clientkey and serverkey.
        private static byte[] GenerateBlake2BNonce(byte[] snonce, byte[] clientKey, byte[] serverKey)
        {
            var hashBuffer = new byte[clientKey.Length + serverKey.Length + snonce.Length];

            Buffer.BlockCopy(snonce, 0, hashBuffer, 0, PublicKeyBox.NonceLength);
            Buffer.BlockCopy(clientKey, 0, hashBuffer, PublicKeyBox.NonceLength, clientKey.Length);
            Buffer.BlockCopy(serverKey, 0, hashBuffer, PublicKeyBox.NonceLength + PublicKeyBox.PublicKeyLength, serverKey.Length);

            using (var blake = new Blake2B(PublicKeyBox.NonceLength))
                return blake.ComputeHash(hashBuffer);
        }

        // Increment nonce by 2.
        private static void IncrementNonce(byte[] nonce)
        {
            ushort c = 2;
            for (int i = 0; i < nonce.Length; i++)
            {
                c += nonce[i];
                nonce[i] = (byte)c;

                // 8 bits right shift to get the carry.
                c >>= 8;
            }
        }
    }
}
