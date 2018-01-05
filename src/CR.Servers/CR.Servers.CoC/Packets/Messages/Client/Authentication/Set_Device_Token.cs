namespace CR.Servers.CoC.Packets.Messages.Client.Authentication
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.Binary;

    internal class Set_Device_Token : Message
    {
        internal string Password;

        public Set_Device_Token(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 10113;

        internal override void Decode()
        {
            this.Password = this.Reader.ReadString();
            this.Reader.ReadInt32();
        }
    }
}