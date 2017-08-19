using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Avatar_Profile : Message
    {
        internal long UserID;

        public Avatar_Profile(Device device, Reader reader) : base(device, reader)
        {
            // Avatar_Profile.
        }

        public override void Decode()
        {
            UserID = Reader.ReadInt64();

            if (Reader.ReadBoolean())
            {
                Reader.ReadInt32(); //HomeId High
                Reader.ReadInt32(); //HomeId Low
            }
        }

        public override void Process()
        {
            new Avatar_Profile_Data(Device) {UserID = UserID}.Send();
        }
    }
}