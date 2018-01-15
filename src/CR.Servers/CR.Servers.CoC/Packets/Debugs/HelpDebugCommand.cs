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
                "/addunits (adds 500 of all units) \n" +
                "/addspells (adds 500 of all spells) \n" +
                //"/own-base (attack yourself) \n" +
                "/clearobstacles (clears all of your existing obstacles) \n" +
                "/resetbase (clears all of your existing buildings and resets your base) \n" +
                "/maxlevels (maximize your existing buildings' level) \n" +
                "/maxresources (refills your storages) \n");
                //"/maxbases (maximizes your bases)");
        }
    }
}