namespace CR.Servers.CoC.Packets.Cryptography
{
    public interface IEncrypter
    {
        bool IsRC4
        {
            get;
        }

        byte[] Decrypt(byte[] Data);
        byte[] Encrypt(byte[] Data);
    }
}
