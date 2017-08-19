using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Mission_Progress : Command
    {
        internal int Mission_ID;

        public Mission_Progress(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }

        public override void Decode()
        {
            Mission_ID = Reader.ReadInt32();
            Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.Player.Avatar.Mission_Finish(Mission_ID))
            {
                // Missions Mission = CSV.Tables.Get(Gamefile.Missions).GetDataWithID(Mission_ID) as Missions;
            }
        }
    }
}
