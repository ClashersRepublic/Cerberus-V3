using Magic.ClashOfClans.Extensions.Binary;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Claim_Achievement : Command
    {
        public Claim_Achievement(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Reader.ReadInt32();
            Reader.ReadInt32();
        }
    }
}
