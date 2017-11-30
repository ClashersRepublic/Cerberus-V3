using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Profile_Data : Message
    {
        internal override short Type => 24334;

        public Avatar_Profile_Data(Device Device, Level Level) : base(Device)
        {
            this.Player = Level;
            this.Player.Tick();
        }

        internal Level Player;

        internal override void Encode()
        {
            this.Player.Player.Encode(this.Data);

            this.Data.AddCompressed(this.Player.Home.HomeJSON.ToString(), false);

            this.Data.AddInt(0); // Troop Received
            this.Data.AddInt(0); // Troop Sended
            this.Data.AddInt(0); // RemainingSecs for available in alliance war

            this.Data.Add(1);

            this.Data.AddInt(0);
        }
    }
}
