namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    using System;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class DuelBattleInfoMessage : Message
    {
        internal Level Enemy;

        public DuelBattleInfoMessage(Device Device, Level Enemy) : base(Device)
        {
            this.Enemy = Enemy;
        }

        internal override short Type => 24372;

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddByte(0);
            this.Data.AddLong(this.Enemy.Player.UserId); //Battleid

            this.Data.AddBool(this.Enemy.Player.AllianceLowId > 0);
            if (this.Enemy.Player.AllianceLowId > 0)
            {
                this.Data.AddLong(this.Enemy.Player.AllianceId);
                this.Data.AddString(this.Enemy.Player.Alliance.Header.Name);
                this.Data.AddInt(this.Enemy.Player.Alliance.Header.Badge);
                this.Data.AddInt(this.Enemy.Player.Alliance.Header.ExpLevel);
            }
            this.Data.AddLong(this.Enemy.Player.UserId);
            this.Data.AddLong(this.Enemy.Player.UserId);
            this.Data.AddString(this.Enemy.Player.Name);

            this.Data.AddInt(2);
            this.Data.AddByte(0);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(0);
            this.Data.AddVInt((int) TimeSpan.FromMinutes(4).TotalSeconds);
        }
    }
}