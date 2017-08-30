using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Ask_Visit_Home : Message
    {
        internal long AvatarId;

        public Ask_Visit_Home(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            AvatarId = Reader.ReadInt64();
        }

        public override void Process()
        {
            var target = ResourcesManager.GetPlayer(AvatarId);
            if (target != null)
            {
                new Visit_Home_Data(Device, target).Send();

                if (Device.Player.Avatar.ClanId > 0)
                {
                    //new Alliance_All_Stream_Entry(this.Device).Send();
                }
            }
            else
            {
                new Own_Home_Data(Device).Send();
            }
        }
    }
}
