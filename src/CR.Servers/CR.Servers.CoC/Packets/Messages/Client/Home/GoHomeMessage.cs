namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class GoHomeMessage : Message
    {
        internal int Mode;

        public GoHomeMessage(Device device, Reader reader) : base(device, reader)
        {
            // Space
        }

        internal override short Type
        {
            get
            {
                return 14101;
            }
        }

        internal override void Decode()
        {
            this.Mode = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            if (this.Mode == 1)
            {
                this.Device.State = State.WAR_EMODE;
            }
            else
            {
                if (this.Device.State == State.IN_PC_BATTLE)
                {
                    if (this.Device.Account.Battle != null)
                    {
                        this.Device.Account.Battle.EndBattle();
                        this.Device.Account.Battle = null;
                    }
                }

                if (this.Device.Account.DuelBattle != null)
                {
                    this.Device.Account.DuelBattle.GetBattle(this.Device.GameMode.Level)?.EndBattle();
                    this.Device.Account.DuelBattle.SendDuelBattleInfoMessage();
                    this.Device.Account.DuelBattle = null;
                }

                this.Device.State = State.LOGGED;
            }

            new OwnHomeDataMessage(this.Device).Send();
        }
    }
}