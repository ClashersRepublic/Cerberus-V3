namespace CR.Servers.CoC.Packets.Messages.Client.Battle
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Logic.Enums;

    internal class CancelDuelMatchmakeMessage : Message
    {
        public CancelDuelMatchmakeMessage(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14103;

        internal override void Process()
        {
            Resources.Duels.Quit(this.Device);
        }
    }
}