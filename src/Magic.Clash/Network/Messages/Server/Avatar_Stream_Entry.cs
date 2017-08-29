using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Avatar_Stream_Entry : Message
    {
        internal Mail Message;

        internal Avatar_Stream_Entry(Device _Device, Mail message) : base(_Device)
        {
            Identifier = 24412;
            Message = message;
        }

        public override void Encode()
        {
            Data.AddRange(Message.ToBytes);
        }
    }
}
