using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.Packets.Messages.Server
{
    internal class RC4SessionKey : Message
    {
        public RC4SessionKey(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(20000);
            Key = Utils.CreateRandomByteArray();
        }
        public override void Encode()
        {
            List<byte> pack = new List<byte>();
            pack.AddByteArray(Key);
            pack.AddInt32(1);
            Encrypt(pack.ToArray());
        }
        public byte[] Key { get; set; }

        public override void Process(Level level)
        {
            Client.UpdateKey(Key);
        }
    }
}