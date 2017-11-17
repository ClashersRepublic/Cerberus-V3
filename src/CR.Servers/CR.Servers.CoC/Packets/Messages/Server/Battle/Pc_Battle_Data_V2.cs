using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Pc_Battle_Data_V2 : Message
    {
        internal override short Type => 25023;

        public Pc_Battle_Data_V2(Device Device) : base(Device)
        {
        }

        internal override void Encode()
        {
            var Level = this.Device.GameMode.Level;
            Level.Player.Encode(this.Data);
            this.Data.AddLong(Level.Player.UserId); //Opponent id
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddCompressed(Level.Battle().ToString());
            this.Data.AddCompressed(Game_Events.Events_Json);
            this.Data.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
        }
    }
}
