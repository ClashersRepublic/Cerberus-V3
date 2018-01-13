namespace CR.Servers.CoC.Packets.Debugs
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Logic.Enums;

    internal class PrintAccountIDDebugCommand : Debug
    {
        public PrintAccountIDDebugCommand(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
            // PrintAccountIDDebugCommand
        }

        internal override Rank RequiredRank => Rank.Player;

        internal override void Process()
        {
            this.SendChatMessage("Your User ID: " + Device.GameMode.Level.Player.UserId.ToString());
        }
    }
}