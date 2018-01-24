using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Packets.Commands.Client.Battle;
using CR.Servers.CoC.Packets.Messages.Server.Home;

namespace CR.Servers.CoC.Packets.Debugs
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class Own_Base_Attack : Debug
    {
        public Own_Base_Attack(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
        }

        internal override Rank RequiredRank
        {
            get
            {
                return Rank.Player;
            }
        }

        internal override void Process()
        {
            if (this.Device.GameMode.Level.Player.ModSlot.SelfAttack)
            {
                this.Device.GameMode.Level.Player.ModSlot.SelfAttack = false;
                this.SendChatMessage("Own village attack disabled!");
            }
            else
            {
                this.Device.GameMode.Level.Player.ModSlot.SelfAttack = true;
                this.SendChatMessage("Own village attack enabled!");

                new AvailableServerCommandMessage(this.Device)
                {
                    Command = new Search_Opponent(this.Device)
                }.Send();
            }
        }
    }
}
