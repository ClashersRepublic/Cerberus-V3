using Magic.Network.Cryptography.NaCl;
using System;

namespace Magic.Network.Cryptography
{
    public partial class MessageCryptoNaCl
    {
        public class Server : MessageCrypto
        {
            public Server(KeyPair keyPair)
            {
                if (keyPair == null)
                    throw new ArgumentNullException(nameof(keyPair));

                _keyPair = keyPair;
            }

            private readonly KeyPair _keyPair;
            private byte[] _k;
            private byte[] _sessionKey;
            private byte[] _clientPk;

            private byte[] _serverNonce;
            private byte[] _clientNonce;

            private int _incoming;
            private int _outgoing;

            public override byte[] ProcessIncoming(byte[] cipher)
            {
                if (cipher == null)
                    throw new ArgumentNullException(nameof(cipher));

                // First message is received unencrypted.
                if (_incoming == 0)
                {
                    _incoming++;
                    return cipher;
                }
                else if (_incoming == 1)
                {
                    _incoming++;

                    // Post encryption.
                    var clientPk = new byte[PublicKeyBox.PublicKeyLength];
                    Buffer.BlockCopy(cipher, 0, clientPk, 0, PublicKeyBox.PublicKeyLength);

                    _clientPk = clientPk;
                    var nonce = GenerateBlake2BNonce(_clientPk, _keyPair.PublicKey);

                    var tmpCipher = new byte[cipher.Length - PublicKeyBox.PublicKeyLength];
                    Buffer.BlockCopy(cipher, PublicKeyBox.PublicKeyLength, tmpCipher, 0, tmpCipher.Length);

                    var tmpPlaintext = PublicKeyBox.Open(tmpCipher, _clientPk, _keyPair.PrivateKey, nonce);

                    // Pre encryption.
                    // snonce.
                    _clientNonce = new byte[PublicKeyBox.NonceLength];
                    _sessionKey = new byte[PublicKeyBox.NonceLength];
                    Buffer.BlockCopy(tmpPlaintext, 0, _sessionKey, 0, PublicKeyBox.NonceLength);
                    Buffer.BlockCopy(tmpPlaintext, PublicKeyBox.NonceLength, _clientNonce, 0, PublicKeyBox.NonceLength);

                    var plaintextLen = tmpPlaintext.Length - (PublicKeyBox.NonceLength * 2);
                    var plaintext = new byte[plaintextLen];
                    Buffer.BlockCopy(tmpPlaintext, (PublicKeyBox.NonceLength * 2), plaintext, 0, plaintextLen);
                    return plaintext;
                }
                else if (_incoming > 1)
                {
                    IncrementNonce(_clientNonce);

                    var plaintext = SecretBox.Open(cipher, _k, _clientNonce);
                    return plaintext;
                }
                return null;
            }

            public override byte[] ProcessOutgoing(byte[] plaintext)
            {
                if (plaintext == null)
                    throw new ArgumentNullException(nameof(plaintext));

                // Second message is sent unencrypted;
                if (_outgoing == 0)
                {
                    _outgoing++;
                    return plaintext;
                }
                else if (_outgoing == 1)
                {
                    _outgoing++;

                    // rnonce.
                    _serverNonce = PublicKeyBox.GenerateNonce();

                    var key = PublicKeyBox.GenerateKeyPair();
                    var nonce = GenerateBlake2BNonce(_clientNonce, _clientPk, _keyPair.PublicKey);

                    var tmpPlaintext = new byte[plaintext.Length + PublicKeyBox.PublicKeyLength + PublicKeyBox.NonceLength];
                    Buffer.BlockCopy(_serverNonce, 0, tmpPlaintext, 0, PublicKeyBox.NonceLength);
                    Buffer.BlockCopy(key.PublicKey, 0, tmpPlaintext, PublicKeyBox.NonceLength, PublicKeyBox.PublicKeyLength);
                    Buffer.BlockCopy(plaintext, 0, tmpPlaintext, PublicKeyBox.NonceLength + PublicKeyBox.PublicKeyLength, plaintext.Length);

                    _k = key.PublicKey;

                    var cipher = PublicKeyBox.Box(tmpPlaintext, _clientPk, _keyPair.PrivateKey, nonce);
                    return cipher;
                }
                else if (_outgoing > 1)
                {
                    IncrementNonce(_serverNonce);

                    var cipher = SecretBox.Box(plaintext, _k, _serverNonce);
                    return cipher;
                }
                return null;
            }
        }
    }
}
