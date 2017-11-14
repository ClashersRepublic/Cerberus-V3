using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Mission_Progress : Command
    {
        internal override int Type => 519;
        internal int Mission_ID;

        public Mission_Progress(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal override void Decode()
        {
            Mission_ID = Reader.ReadInt32();
            ExecuteSubTick = Reader.ReadInt32();
        }

        internal override void Execute()
        {
            if (Device.Account.Player.Mission_Finish(Mission_ID))
            {
                // Missions Mission = CSV.Tables.Get(Gamefile.Missions).GetDataWithID(Mission_ID) as Missions;
            }
        }
    }
}
