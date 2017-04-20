namespace Magic.Network.Cryptography
{
    public abstract class MessageCrypto
    {
        public abstract byte[] ProcessIncoming(byte[] cipher);

        public abstract byte[] ProcessOutgoing(byte[] plaintext);
    }
}
