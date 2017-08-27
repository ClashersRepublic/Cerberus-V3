using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class My_League : Command
    {
        public My_League(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();
        }

        public override void Process()
        {
            //new League_Players(this.Device).Send();
        }
    }
}
