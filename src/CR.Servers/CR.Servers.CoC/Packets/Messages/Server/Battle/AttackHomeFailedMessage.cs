namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;
    using CR.Servers.Logic.Enums;

    internal class AttackHomeFailedMessage : Message
    {
        public AttackHomeFailedMessage(Device Device) : base(Device)
        {
        }

        internal override short Type => 24103;
    }
}