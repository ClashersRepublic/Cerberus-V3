namespace CR.Servers.CoC.Packets.Debugs
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class HelpDebugCommand : Debug
    {
        public HelpDebugCommand(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
            // HelpDebugCommand
        }

        internal override Rank RequiredRank => Rank.Player;

        internal override void Process()
        {
            SendChatMessage("~ Available Server Commands ~ \n" +
                "/help (lists available server commands) \n" +
                "/id (prints your Account ID) \n" +
                "/own-base (attack yourself) \n" +
                "/clear-obstacles (clears all of your existing obstacles) \n" +
                "/clear-buildings (clears all of your existing buildings) \n" +
                "/max-buildings (maximize your existing buildings' level) \n" +
                "/max-resources (refills your storages) \n" +
                "/max-bases (maximizes your bases)");
        }
    }
}