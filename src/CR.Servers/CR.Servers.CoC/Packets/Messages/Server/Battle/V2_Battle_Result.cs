
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Battle;
using CR.Servers.CoC.Logic.Battle.Slots;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class V2_Battle_Result : Message
    {
        internal override short Type => 24371;
        internal Battles_V2 Battle;
        internal Battle_V2 Home;
        internal Battle_V2 Enemy;

        public V2_Battle_Result(Device device, Battles_V2 battle) : base(device)
        {
            this.Battle = battle;
            this.Home = this.Battle.GetPlayerBattle(this.Device.GameMode.Level.Player.UserId);
            this.Enemy = this.Battle.GetEnemyBattle(this.Device.GameMode.Level.Player.UserId);
        }

        internal override void Encode()
        {
            //this.Data.AddHexa("00 00 00 01 00 00 00 00 00 04 36 9F BE");

            this.Data.AddInt(1);
            this.Data.AddByte(0);

            this.Data.AddLong(this.Enemy.Attacker.UserId); //Battleid

            this.Data.AddBool(this.Enemy.Attacker.InAlliance);
            if (this.Enemy.Attacker.InAlliance)
            {
                this.Data.AddLong(this.Enemy.Attacker.AllianceId);
                this.Data.AddString(this.Enemy.Attacker.Alliance.Header.Name);
                this.Data.AddInt(this.Enemy.Attacker.Alliance.Header.Badge);
                this.Data.AddInt(this.Enemy.Attacker.Alliance.Header.ExpLevel);
            }
            this.Data.AddLong(this.Enemy.Attacker.UserId);
            this.Data.AddLong(this.Enemy.Attacker.UserId);
            this.Data.AddString(this.Enemy.Attacker.Name);
            this.Data.AddInt(2);
            this.Data.AddByte(0);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(15000);
            this.Data.AddVInt(0);
            this.Data.AddVInt((int)this.Enemy.BattleTick); //Opponent time left
            this.Data.AddBool(this.Enemy.Finished && (this.Home.ReplayInfo.Stats.DestructionPercentage > this.Enemy.ReplayInfo.Stats.DestructionPercentage));

            this.Data.AddInt(this.Home.ReplayInfo.Stats.AttackerStars);
            this.Data.AddInt(this.Home.ReplayInfo.Stats.DestructionPercentage); //Home percentage

            this.Data.AddInt(this.Enemy.Finished ? this.Enemy.ReplayInfo.Stats.AttackerStars : 0);
            this.Data.AddInt(this.Enemy.Finished ? this.Enemy.ReplayInfo.Stats.DestructionPercentage : 0); //Enemy percentage
            this.Data.AddInt(0); //Win or lost?
            this.Data.AddInt(0); //Win or lost?
            this.Data.AddInt(0); //Win or lost trophies

            this.Data.AddByte(this.Enemy.Finished ? 3 : 2);
            this.Data.AddInt(2); //Replay Low ID
            this.Data.AddInt(0);
            this.Data.AddInt(0); //Replay High ID
            this.Data.AddInt(8); //Major
            this.Data.AddInt(24);//Minor
            this.Data.AddInt(5); //Build

            this.Data.AddBool(this.Enemy.Finished);
            if (this.Enemy.Finished)
            {
                this.Data.AddInt(1); //Replay Low ID
                this.Data.AddInt(0);
                this.Data.AddInt(0); //Replay High ID
                this.Data.AddInt(8); //Major
                this.Data.AddInt(24); //Minor
                this.Data.AddInt(5); //Build
            }
            this.Data.AddInt(31);

            this.Data.AddString(this.Home.ReplayInfo.Save().ToString());
            this.Data.AddString(this.Enemy.Finished ? this.Enemy.ReplayInfo.Save().ToString() : null);
        }
    }
}
