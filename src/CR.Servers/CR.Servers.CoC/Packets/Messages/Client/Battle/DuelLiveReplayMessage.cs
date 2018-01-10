namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class DuelLiveReplayMessage : Message
    {
        internal int HighId;
        internal int LowId;

        public DuelLiveReplayMessage(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type
        {
            get
            {
                return 15110;
            }
        }

        internal override void Decode()
        {
            this.HighId = this.Reader.ReadInt32();
            this.LowId = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            /*
            Player Player = Resources.Accounts.LoadAccount(this.HighId, this.LowId).Player;

            if (Player != null)
            {
                if (Player.Level?.BattleManager != null)
                {
                    Player.Level.BattleManager.AddSpectator(this.Device.GameMode.Level);
                }
            }
            */
        }
    }
}