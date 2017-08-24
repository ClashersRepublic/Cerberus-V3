using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class New_Shop_Items_Seen : Command
    {
        public New_Shop_Items_Seen(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();
        }
    }
}