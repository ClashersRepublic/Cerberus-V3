using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class Request_Name_Change : Message
    {
        internal string Name;

        public Request_Name_Change(Device device, Reader reader) : base(device, reader)
        {
            // Request_Name_Change.
        }

        public override void Decode()
        {
            Name = Reader.ReadString();
        }

        public override void Process()
        {
            new Name_Change_Response(Device, Name).Send();
        }
    }
}
