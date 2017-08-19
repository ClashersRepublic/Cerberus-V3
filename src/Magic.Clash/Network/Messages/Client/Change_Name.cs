using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Change_Name : Message
    {
        internal string Name;

        public Change_Name(Device device, Reader reader) : base(device, reader)
        {
            // Change_Name.
        }

        public override void Decode()
        {
            Name = Reader.ReadString();
            Reader.ReadByte();
        }

        public override void Process()
        {
            if (!string.IsNullOrEmpty(Name) && Name.Length < 16)
                //if (!GameTools.Badwords.Contains(this.Name))
            {
                Device.Player.Avatar.Name = Name;
                Device.Player.Avatar.NameState += 1;

                new Server_Commands(Device, new Name_Change_Callback(Device)).Send();
            }
            // else
            //   new Change_Name_Failed(this.Device).Send();
        }
    }
}
