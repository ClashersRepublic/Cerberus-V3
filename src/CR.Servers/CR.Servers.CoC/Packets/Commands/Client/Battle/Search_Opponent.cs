namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    using System;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class Search_Opponent : Command
    {
        public Search_Opponent(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override int Type => 800;

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
                if (this.Device.State == State.IN_PC_BATTLE)
                {
                    if (this.Device.Account.DefenseAccount != null)
                    {
                        this.Device.Account.DefenseAccount.InBattle = false;
                        this.Device.Account.DefenseAccount = null;
                    }

                    this.Device.Account.InBattle = false;

                    this.Device.State = State.LOGGED;
                }

                Account rndAccount = Resources.Accounts.LoadRandomAccount();

                if (rndAccount != null)
                {
                    rndAccount.InBattle = true;
                    rndAccount.StartBattleTime = DateTime.UtcNow;

                    this.Device.Account.InBattle = true;
                    this.Device.Account.DefenseAccount = rndAccount;
                    this.Device.Account.StartBattleTime = DateTime.UtcNow;

                    Level defenseLevel = new Level();

                    defenseLevel.SetPlayer(rndAccount.Player);
                    defenseLevel.SetHome(rndAccount.Home);

                    new Enemy_Home_Data(this.Device, defenseLevel).Send();
                }
                else
                {
                    Logging.Error(this.GetType(), "Matchmaking Failed.");
                }
            }
        }
    }
}