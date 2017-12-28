using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Battle.Manager;
using CR.Servers.CoC.Packets.Messages.Server.Battle;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Search_Opponent : Command
    {
        internal override int Type => 800;

        public Search_Opponent(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            if (this.Device.GameMode.Level.Player.ModSlot.AIAttack)
            {
                new Enemy_Home_Data(this.Device).Send();
            }
            else
            {
                //Get a random village

                new Enemy_Home_Data(this.Device, this.Device.GameMode.Level).Send();
            }
        }
    }
}
