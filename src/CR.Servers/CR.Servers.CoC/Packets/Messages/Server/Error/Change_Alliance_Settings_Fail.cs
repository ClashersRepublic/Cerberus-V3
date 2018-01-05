namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Enums;
    using CR.Servers.Extensions.List;

    internal class Change_Alliance_Settings_Fail : Message
    {
        public Change_Alliance_Settings_Fail(Device Device) : base(Device)
        {
        }

        internal override short Type => 24333;


        internal override void Encode()
        {
            this.Data.AddInt((int) AllianceErrorReason.InvalidDescription);
        }
    }
}