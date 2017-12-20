using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class Set_Device_Token : Message
    {
        internal override short Type => 10113;

        public Set_Device_Token(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal string Password;

        internal override void Decode()
        {
            this.Password = this.Reader.ReadString();
            this.Reader.ReadInt32();
        }
    }
}
