using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Profile_Data : Message
    {
        internal override short Type => 24334;

        public Avatar_Profile_Data(Device Device, Player Player, Logic.Home Home) : base(Device)
        {
            this.Player = Player;
            this.Home = Home;
        }

        public Avatar_Profile_Data(Device Device) : base(Device)
        {
        }

        internal Player Player;
        internal Logic.Home Home;

        internal override void Encode()
        {
            this.Player.Encode(this.Data);

            this.Data.AddCompressed(this.Home.HomeJSON.ToString(), false);

            this.Data.AddInt(0); // Troop Received
            this.Data.AddInt(0); // Troop Sended
            this.Data.AddInt(0); // RemainingSecs for available in alliance war

            this.Data.Add(1);

            this.Data.AddInt(0);
        }
    }
}
