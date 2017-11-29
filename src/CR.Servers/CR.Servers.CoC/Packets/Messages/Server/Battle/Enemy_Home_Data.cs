using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Enemy_Home_Data : Message
    {
        internal override short Type => 24107;

        public Enemy_Home_Data(Device Device, Player Player, Logic.Home Home) : base(Device)
        {
            this.Enemy = new Level();
            this.Enemy.SetPlayer(Player);
            this.Enemy.SetHome(Home);
            this.Enemy.FastForwardTime(0);
            this.Enemy.Process();

            this.Enemy.Tick();
        }

        public Enemy_Home_Data(Device Device, Level Enemy) : base(Device)
        {
            this.Enemy = Enemy;
            this.Enemy.Tick();
        }


        internal Level Enemy;

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Data.AddInt(-1);

            this.Enemy.Home.Encode(this.Data);
            this.Enemy.Player.Encode(this.Data);
            this.Device.GameMode.Level.Player.Encode(this.Data);

            this.Data.AddInt(3);
            this.Data.AddInt(0);
            this.Data.AddByte(0);
        }

        internal override void Process()
        {
            this.Device.State = State.IN_PC_BATTLE;
        }
    }
}
