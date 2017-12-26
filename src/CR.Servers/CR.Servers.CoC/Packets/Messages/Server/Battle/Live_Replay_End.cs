using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Live_Replay_End : Message
    {
        internal override short Type => 24126;

        public Live_Replay_End(Device Device) : base(Device)
        {
            this.Version = 9;
        }
    }
}
