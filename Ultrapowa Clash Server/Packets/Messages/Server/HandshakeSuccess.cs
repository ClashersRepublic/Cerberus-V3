using System.Collections.Generic;
using UCS.Core.Crypto;
using UCS.Core.Crypto.Blake2b;
using UCS.Helpers;
using UCS.PacketProcessing.Messages.Client;

namespace UCS.PacketProcessing.Messages.Server
{
    // Packet 20100
    internal class HandshakeSuccess : Message
    {
        private byte[] _sessionKey;
        private static readonly Hasher Blake = Blake2B.Create(new Blake2BConfig { OutputSizeInBytes = 24 });

        public HandshakeSuccess(PacketProcessing.Client client, HandshakeRequest cka) : base(client)
        {
            SetMessageType(20100);
            Blake.Init();
            Blake.Update(Key.Crypto.PrivateKey);
            _sessionKey = Blake.Finish();
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddByteArray(_sessionKey);
            SetData(pack.ToArray());
        }
    }
}
