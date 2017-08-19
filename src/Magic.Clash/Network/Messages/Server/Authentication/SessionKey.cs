using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server.Authentication
{
    internal class SessionKey : Message
    {
        public SessionKey(Device device) : base(device)
        {
            Identifier = 20000;
            Key = Utils.CreateRandomByteArray();
        }
        public override void Encode()
        {
            Data.AddByteArray(Key);
            Data.AddInt(1);
        }
        public byte[] Key { get; set; }

        public override void Process()
        {
            Device.UpdateKey(Key);
        }
    }
}