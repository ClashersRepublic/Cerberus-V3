using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Avatar_Stream : Message
    {
        internal Avatar Player;

        public Avatar_Stream(Device client) : base(client)
        {
            Identifier = 24411;
        }

        public Avatar_Stream(Device client, Avatar player) : base(client)
        {
            Identifier = 24411;
            Player = player;
        }

        public override void Encode()
        {
            Data.AddRange(Player != null ? Player.Inbox.ToBytes : Device.Player.Avatar.Inbox.ToBytes);
        }
    }
}
