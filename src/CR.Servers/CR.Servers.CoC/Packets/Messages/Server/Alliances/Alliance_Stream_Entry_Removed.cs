namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Alliance_Stream_Entry_Removed : Message
    {
        internal long MessageId;

        public Alliance_Stream_Entry_Removed(Device Device) : base(Device)
        {
        }

        internal override short Type => 24318;

        internal override void Encode()
        {
            this.Data.AddLong(this.MessageId);
        }
    }
}