using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Network.Messages.Server.Battle;

namespace Magic.ClashOfClans.Network.Messages.Client.Battle
{
    internal class Search_Opponent : Message
    {
        public Search_Opponent(Device device, Reader reader) : base(device, reader)
        {
            // Search_Opponent.
        }

        public override void Process()
        {
            Device.Player.Avatar.Last_Attack_Enemy_ID.Clear();
            new Battle_Failed(Device).Send(); //No idea how to reply yet
        }
    }
}
