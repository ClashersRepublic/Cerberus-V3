namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class Cancel_V2_Battle : Message
    {
        public Cancel_V2_Battle(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14103;

        internal override void Process()
        {
            /*
            if (this.Device.State == State.SEARCH_BATTLE)
            {
                Resources.BattlesV2.Dequeue(this.Device.GameMode.Level);
            }
            */

            this.Device.State = State.LOGGED;
            new Own_Home_Data(this.Device).Send();
        }
    }
}