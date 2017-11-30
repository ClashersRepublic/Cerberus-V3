using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    internal class Visit_Home_Data : Message
    {
        internal override short Type => 24113;

        internal Visit_Home_Data(Device Device, Level Player) : base(Device)
        {
            this.Visit = Player;
            this.Visit.Tick();
            this.Device.State = State.VISIT;
        }

        internal Level Visit;

        internal override void Encode()
        {
            this.Data.AddInt(0);
            this.Visit.Home.Encode(this.Data);
            this.Visit.Player.Encode(this.Data);
            this.Data.AddInt(0);
            this.Data.AddByte(1);
            this.Device.GameMode.Level.Player.Encode(this.Data);
        }
    }
}
