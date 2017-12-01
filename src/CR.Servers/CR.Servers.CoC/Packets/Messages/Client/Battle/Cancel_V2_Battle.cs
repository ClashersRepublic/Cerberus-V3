using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    internal class Cancel_V2_Battle : Message
    {
        internal override short Type => 14103;

        public Cancel_V2_Battle(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Process()
        {
            if (Device.State == State.IN_1VS1_BATTLE) 
                Device.State = State.LOGGED;

            new Own_Home_Data(this.Device).Send();
        }
    }
}
