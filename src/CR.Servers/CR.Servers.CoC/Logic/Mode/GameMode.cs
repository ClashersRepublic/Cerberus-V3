using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic.Mode.Enums;

namespace CR.Servers.CoC.Logic.Mode
{
    internal class GameMode
    {
        internal Device Device;
        internal Level Level;

        internal Time Time;
        internal State State;

        /*internal CommandManager CommandManager;
        internal GameLogManager GameLogManager;*/

        internal bool Connected => this.Device != null && this.Device.Connected;

        public GameMode(Device Device)
        {
            this.Device = Device;
            this.Time = new Time();
            this.Level = new Level(this);

           /* this.CommandManager = new CommandManager(this.Level);
            this.GameLogManager = new GameLogManager(this);*/
        }

        internal void LoadHomeState(Home Home, Player Player)
        {
            Console.WriteLine(Home.HomeJSON);

            this.Time = new Time();
            this.State = State.Home;

            this.Level.SetPlayer(Player);
            this.Level.SetHome(Home);
            //this.Level.FastForwardTime(Player.s);
            this.Level.Process();
        }
        /*

        internal void LoadNpcAttackState(PlayerBase NpcPlayer, Home Home, PlayerBase VisitorPlayer, int Timestamp, int SecondsSinceLastSave)
        {
            if (this.State <= 0)
            {
                if (this.Level != null)
                {
                    this.Level = null;
                }

                this.Time = new Time();
                this.State = State.Attack;
                this.Timestamp = Timestamp;

                this.Level = new Level(this);
                this.Level.SetPlayer(NpcPlayer);
                this.Level.SetHome(Home);
                this.Level.SetVistorPlayer(VisitorPlayer);
                this.Level.FastForwardTime(SecondsSinceLastSave);
                this.Level.LoadingFinished();
            }
        }

        internal void EndDefendState()
        {
            if (this.State == State.Defend)
            {
                this.State = State.Home;
                this.Level.DefenseStateEnded();
            }
            else
                Logging.Error(this.GetType(), "EndDefendState called from invalid state");
        }
        internal void StartDefendState(Player Attacker)
        {
            if (this.State == State.Home || this.State == State.Defend)
            {
                this.State = State.Defend;
                this.Level.DefenseStateStarted(Attacker);
            }
            else
                Logging.Error(this.GetType(), "StartDefendState called from invalid state");
        }*/
    }
}
