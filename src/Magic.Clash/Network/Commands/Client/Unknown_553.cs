using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Unknown_553 : Command
    {
        public Unknown_553(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Reader.ReadInt32();
            Reader.ReadInt32(); //Tick
        }
    }
}