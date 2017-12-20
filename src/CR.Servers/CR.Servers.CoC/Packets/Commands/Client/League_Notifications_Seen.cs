using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class League_Notifications_Seen : Command
    {
        internal override int Type => 538;
        public League_Notifications_Seen(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int League;
        internal int Unknown;

        internal override void Decode()
        {
            this.League = this.Reader.ReadInt32();
            this.Unknown = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            ShowValues();
            var Level = this.Device.GameMode.Level;

            Level.LastLeagueRank = this.League;
            Level.LastLeagueShuffleInfo = 0;
        }
    }
}
