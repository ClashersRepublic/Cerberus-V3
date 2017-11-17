namespace CR.Servers.CoC.Logic.Mode
{
    internal class GameMode
    {
        internal Device Device;
        internal Level Level;

        internal Time Time;
        //internal State State;

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
            this.Time = new Time();
            //this.State = State.Home;

            this.Level.SetPlayer(Player);
            this.Level.SetHome(Home);
            //this.Level.FastForwardTime(Player.s);
            this.Level.Process();
        }
      
    }
}
