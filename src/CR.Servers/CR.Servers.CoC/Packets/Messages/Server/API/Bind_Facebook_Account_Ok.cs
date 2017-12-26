using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.API
{
    internal class Bind_Facebook_Account_Ok : Message
    {
        internal override short Type => 14201;

        public Bind_Facebook_Account_Ok(Device Device) : base(Device)
        {

        }

        internal override void Encode()
        {
            this.Data.AddInt(1);
        }
    }
}
