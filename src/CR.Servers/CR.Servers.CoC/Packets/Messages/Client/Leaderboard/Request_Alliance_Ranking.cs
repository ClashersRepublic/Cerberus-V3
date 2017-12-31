using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Leaderboard
{
    internal class Request_Alliance_Ranking : Message
    {
        internal override short Type => 14401;

        public Request_Alliance_Ranking(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal bool HasLong;
        internal long UnknownLong;
        internal bool Local;
        internal int VillageType;
         
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
