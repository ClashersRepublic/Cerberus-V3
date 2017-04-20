﻿using Magic.Network.Cryptography.NaCl.Internal;
using System;
using System.Security.Cryptography;

namespace Magic.Network.Cryptography.NaCl
{
    public static class SecretBox
    {
        public static byte[] Box(byte[] message, byte[] key, byte[] nonce)
        {
            var paddedChiper = new byte[message.Length + xsalsa20poly1305.crypto_secretbox_ZEROBYTES];
            var paddedMessage = new byte[message.Length + xsalsa20poly1305.crypto_secretbox_ZEROBYTES];
            Buffer.BlockCopy(message, 0, paddedMessage, xsalsa20poly1305.crypto_secretbox_ZEROBYTES, message.Length);

            if (xsalsa20poly1305.crypto_secretbox(paddedChiper, paddedMessage, paddedMessage.Length, nonce, key) != 0)
                throw new CryptographicException("Failed to box SecretBox.");

            var chiperLen = paddedChiper.Length - xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES;
            var chiper = new byte[chiperLen];
            Buffer.BlockCopy(paddedChiper, xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES, chiper, 0, chiperLen);
            return chiper;
        }

        public static byte[] Open(byte[] cipher, byte[] key, byte[] nonce)
        {
            var paddedMessage = new byte[cipher.Length + xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES];
            var paddedChiper = new byte[cipher.Length + xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES];
            Buffer.BlockCopy(cipher, 0, paddedChiper, xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES, cipher.Length);

            if (xsalsa20poly1305.crypto_secretbox_open(paddedMessage, paddedChiper, paddedChiper.Length, nonce, key) != 0)
                throw new CryptographicException("Failed to box SecretBox.");

            var messageLen = cipher.Length - xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES;
            var message = new byte[messageLen];
            Buffer.BlockCopy(paddedMessage, xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES, message, 0, messageLen);
            return message;
        }
    }
}
