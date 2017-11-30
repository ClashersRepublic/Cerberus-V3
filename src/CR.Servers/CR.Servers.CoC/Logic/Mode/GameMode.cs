using CR.Servers.CoC.Logic.Manager;

namespace CR.Servers.CoC.Logic.Mode
{
    internal class GameMode
    {
        internal Device Device;
        internal Level Level;

        internal Time Time;
        internal CommandManager CommandManager;
        //internal State State;

        /*
        internal GameLogManager GameLogManager;*/

        internal bool Connected => this.Device != null && this.Device.Connected;

        public GameMode(Device Device)
        {
            this.Device = Device;
            this.Time = new Time();
            this.CommandManager = new CommandManager(this.Level);
            /*this.GameLogManager = new GameLogManager(this);*/
        }

        internal void LoadLevel(Level Level)
        {
            this.Level = Level;
            this.Level.SetGameMode(this);
        }
      
    }
}
