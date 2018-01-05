namespace CR.Servers.CoC.Packets.Messages.Client.Home
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class Inbox_Opened : Message
    {
        public Inbox_Opened(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 10905;

        internal override void Decode()
        {
            this.Reader.ReadVInt();
        }
    }
}