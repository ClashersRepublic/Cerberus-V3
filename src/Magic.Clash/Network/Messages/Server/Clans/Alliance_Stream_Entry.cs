using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Stream_Entry : Message
    {
        internal Entry Message;

        internal Alliance_Stream_Entry(Device _Device, Entry message) : base(_Device)
        {
            Identifier = 24312;
            Message = message;
        }

        public override void Encode()
        {
            Data.AddRange(Message.ToBytes());
        }
    }
}
