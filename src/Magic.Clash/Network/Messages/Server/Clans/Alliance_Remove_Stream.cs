using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Remove_Stream : Message
    {
        internal long Message_ID = 0;

        internal Alliance_Remove_Stream(Device _Device) : base(_Device)
        {
            Identifier = 24318;
        }

        public override void Encode()
        {
            Data.AddLong(Message_ID);
        }
    }
}
