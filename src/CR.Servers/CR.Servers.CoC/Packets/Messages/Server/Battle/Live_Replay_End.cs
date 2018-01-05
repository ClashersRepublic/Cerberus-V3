namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    using CR.Servers.CoC.Logic;

    internal class Live_Replay_End : Message
    {
        public Live_Replay_End(Device Device) : base(Device)
        {
            this.Version = 9;
        }

        internal override short Type => 24126;
    }
}