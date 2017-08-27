using Magic.Royale.Core.Crypto;
using Magic.Royale.Extensions.List;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Network.Messages.Server.Authentication
{
    internal class Pre_Authentication_OK : Message
    {
        internal Pre_Authentication_OK(Device Device) : base(Device)
        {
            Identifier = 20100;
            this.Device.State = State.SESSION_OK;
        }

        public override void Encode()
        {
            Data.AddByteArray(Key.NonceKey);
        }
    }
}