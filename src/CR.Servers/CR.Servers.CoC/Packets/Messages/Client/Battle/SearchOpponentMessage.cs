namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Battles;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class SearchOpponentMessage : Message
    {
        public SearchOpponentMessage(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14123;
        
        internal override void Process()
        {
            if (this.Device.State == State.IN_PC_BATTLE)
            {
                this.Device.Account.Battle.EndBattle();
                this.Device.Account.Battle = null;

                this.Device.State = State.LOGGED;
            }

            Account rndAccount = Resources.Accounts.LoadRandomOfflineAccount();

            if (rndAccount != null)
            {
                Battle battle = new Battle(this.Device.GameMode.Level, rndAccount.Home.Level);

                this.Device.Account.Battle = battle;
                rndAccount.Battle = battle;

                new EnemyHomeDataMessage(this.Device, battle.Defender).Send();
            }
            else
            {
                new AttackHomeFailedMessage(this.Device).Send();
            }
        }
    }
}