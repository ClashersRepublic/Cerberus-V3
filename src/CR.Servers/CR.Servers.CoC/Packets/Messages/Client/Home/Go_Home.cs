using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    internal class Go_Home : Message
    {
        internal override short Type => 14101;

        public Go_Home(Device device, Reader reader) : base(device, reader)
        {
            // Space
        }

        internal int Mode;

        internal override void Decode()
        {
            Mode = Reader.ReadInt32();
        }

        internal override void Process()
        {
            if (Mode == 1)
            {
                Device.State = State.WAR_EMODE;
            }
            else
            {
                if (Device.State == State.IN_PC_BATTLE) //Replay
                    Device.State = State.LOGGED;
                else if (Device.State == State.IN_AMICAL_BATTLE) //Stream
                    Device.State = State.LOGGED;
                else if (Device.State == State.SEARCH_BATTLE) //Search battle
                    Device.State = State.LOGGED;
                else if (Device.State == State.IN_1VS1_BATTLE) //Builder replay
                    Device.State = State.LOGGED;
            }

            new Own_Home_Data(Device).Send();
        }
    }
}
