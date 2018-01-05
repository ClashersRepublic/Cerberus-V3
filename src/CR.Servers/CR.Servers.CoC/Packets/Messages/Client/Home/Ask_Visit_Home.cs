namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Extensions.Binary;

    internal class Ask_Visit_Home : Message
    {
        internal int HighId;
        internal int LowId;

        public Ask_Visit_Home(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override short Type => 14113;

        internal override void Decode()
        {
            this.HighId = this.Reader.ReadInt32();
            this.LowId = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            Player Player = Resources.Accounts.LoadAccount(this.HighId, this.LowId).Player;

            if (Player.Level != null)
            {
                new Visit_Home_Data(this.Device, Player.Level).Send();
            }
            else
            {
                new Own_Home_Data(this.Device).Send();
            }
        }
    }
}