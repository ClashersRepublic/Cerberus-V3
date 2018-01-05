namespace CR.Servers.CoC.Packets.Messages.Client.Leaderboard
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class Request_Local_Player_Ranking : Message
    {
        internal bool HasLong;
        internal long UnknownLong;
        internal int VillageType;

        public Request_Local_Player_Ranking(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14404;

        internal override void Decode()
        {
            this.HasLong = this.Reader.ReadBoolean();

            if (this.HasLong)
            {
                this.UnknownLong = this.Reader.ReadInt32();
            }

            this.VillageType = this.Reader.ReadInt32();
        }
    }
}