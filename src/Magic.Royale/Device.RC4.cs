using Magic.Royale.Core.Crypto;
using System;
using System.Collections.Generic;

namespace Magic.Royale
{
    internal partial class Device
    {
        public void Decrypt(IList<byte> data)
        {
            EnDecrypt(IncomingPacketsKey, data);
        }

        public void Encrypt(IList<byte> data)
        {
            EnDecrypt(OutgoingPacketsKey, data);
        }

        private static void TransformSessionKey(int clientSeed, IList<byte> sessionKey)
        {
            var buffer = new int[624];
            Initialize_generator(clientSeed, buffer);
            var byte100 = 0;
            for (var i = 0; i < 100; i++)
                byte100 = Extract_number(buffer, i);

            for (var i = 0; i < sessionKey.Count; i++)
                sessionKey[i] ^= (byte)(Extract_number(buffer, i + 100) & byte100);
        }

        private static void Initialize_generator(int seed, IList<int> buffer)
        {
            buffer[0] = seed;
            for (var i = 1; i < 624; ++i)
                buffer[i] = 1812433253 * ((buffer[i - 1] ^ (buffer[i - 1] >> 30)) + 1);
        }

        private static int Extract_number(IList<int> buffer, int ix)
        {
            if (ix == 0)
                generate_numbers(buffer);

            var y = buffer[ix];
            y ^= y >> 11;
            y ^= (int)((y << 7) & 2636928640);
            y ^= (int)((y << 15) & 4022730752);
            y ^= y >> 18;

            if ((y & (1 << 31)) != 0)
                y = ~y + 1;

            //ix = (ix + 1) % 624;
            return y % 256;
        }

        private static void generate_numbers(IList<int> buffer)
        {
            for (var i = 0; i < 624; i++)
            {
                var y = (int)((buffer[i] & 0x80000000) + (buffer[(i + 1) % 624] & 0x7fffffff));
                buffer[i] = buffer[(i + 397) % 624] ^ (y >> 1);
                if (y % 2 != 0)
                    buffer[i] = (int)(buffer[i] ^ 2567483615);
            }
        }

        public unsafe void UpdateKey(byte[] sessionKey)
        {
            TransformSessionKey((int)ClientSeed, sessionKey);

            var newKey = new byte[264];
            var clientKey = sessionKey;
            var v7 = Key._RC4_PrivateKey.Length;
            var v9 = Key._RC4_PrivateKey.Length + sessionKey.Length;
            var completeSessionKey = new byte[Key._RC4_PrivateKey.Length + sessionKey.Length];
            Array.Copy(Key._RC4_PrivateKey, 0, completeSessionKey, 0, v7);
            Array.Copy(clientKey, 0, completeSessionKey, v7, sessionKey.Length);
            uint v11 = 0;

            fixed (byte* v5 = newKey, v10 = completeSessionKey)
            {
                do
                {
                    *(v5 + v11 + 8) = (byte)v11;
                    ++v11;
                } while (v11 != 256);
                *v5 = 0;
                *(v5 + 4) = 0;
                while (true)
                {
                    uint v16 = *v5;

                    var v12 = *(v10 + v16 % v9) + *(uint*)(v5 + 4);
                    *(uint*)v5 = v16 + 1;
                    var v13 = *(v5 + v16 + 8);
                    uint v14 = (byte)(v12 + *(v5 + v16 + 8));
                    *(uint*)(v5 + 4) = v14;
                    var v15 = v5 + v14;
                    *(v5 + v16 + 8) = *(v15 + 8);
                    *(v15 + 8) = v13;
                    if (v16 == 255)
                        break;
                }
                uint v17 = 0;
                *v5 = 0;
                *(v5 + 4) = 0;
                while (v17 < v9)
                {
                    ++v17;
                    var v18 = *(uint*)(v5 + 4);
                    var v19 = (byte)(*(uint*)v5 + 1);
                    *(uint*)v5 = v19;
                    var v20 = v5 + v19;
                    var v21 = *(v20 + 8);
                    uint v22 = (byte)(v18 + v21);
                    *(uint*)(v5 + 4) = v22;
                    var v23 = v5 + v22;
                    *(v20 + 8) = *(v23 + 8);
                    *(v23 + 8) = v21;
                }
            }
            Array.Copy(newKey, IncomingPacketsKey, newKey.Length);
            Array.Copy(newKey, OutgoingPacketsKey, newKey.Length);
        }

        private static void EnDecrypt(IList<byte> key, IList<byte> data)
        {
            if (data != null)
            {
                var dataLen = data.Count;

                if (dataLen >= 1)
                    do
                    {
                        dataLen--;
                        var index = (byte)(key[0] + 1);
                        key[0] = index;
                        var num2 = (byte)(key[4] + key[index + 8]);
                        key[4] = num2;
                        var num3 = key[index + 8];
                        key[index + 8] = key[num2 + 8];
                        key[key[4] + 8] = num3;
                        var num4 = key[(byte)(key[key[4] + 8] + key[key[0] + 8]) + 8];
                        data[data.Count - dataLen - 1] = (byte)(data[data.Count - dataLen - 1] ^ num4);
                    }
                    while (dataLen > 0);
            }
        }
    }
}

