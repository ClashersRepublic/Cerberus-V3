namespace CR.Servers.CoC.Packets.Messages.Client.Leaderboard
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class Request_Alliance_Ranking : Message
    {
        internal bool HasLong;
        internal bool Local;
        internal long UnknownLong;
        internal int VillageType;

        public Request_Alliance_Ranking(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14401;

        internal override void Decode()
        {
            this.HasLong = this.Reader.ReadBooleanV2();

            if (this.HasLong)
            {
                this.UnknownLong = this.Reader.ReadInt64();
            }

            this.Local = this.Reader.ReadBooleanV2();
            this.VillageType = this.Reader.ReadInt32();
        }
    }
}