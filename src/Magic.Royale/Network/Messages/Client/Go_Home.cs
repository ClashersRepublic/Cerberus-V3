using Magic.Royale.Extensions.Binary;
using Magic.Royale.Network.Messages.Server;

namespace Magic.Royale.Network.Messages.Client
{
    internal class Go_Home : Message
    {
        public int State;

        public Go_Home(Device device, Reader reader) : base(device, reader)
        {
            // Space
        }

        public override void Decode()
        {
            State = Reader.ReadInt32();
        }

        public override void Process()
        {
            if (State == 1)
            {
                Device.State = Logic.Enums.State.WAR_EMODE;
            }
            else
            {
                if (Device.State == Logic.Enums.State.IN_PC_BATTLE) //Replay
                    Device.State = Logic.Enums.State.LOGGED; 
                else if (Device.State == Logic.Enums.State.IN_AMICAL_BATTLE) //Stream
                    Device.State = Logic.Enums.State.LOGGED;
                else if (Device.State == Logic.Enums.State.SEARCH_BATTLE) //Search battle
                    Device.State = Logic.Enums.State.LOGGED;
                else if (Device.State == Logic.Enums.State.IN_1VS1_BATTLE) //Builder replay
                    Device.State = Logic.Enums.State.LOGGED;
            }

            new Own_Home_Data(Device).Send();

            if (Device.Player.Avatar.ClanId > 0)
            {
            }
        }
    }
}
