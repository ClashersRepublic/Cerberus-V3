namespace CR.Servers.CoC.Packets.Messages.Server.API
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Bind_Facebook_Account_Ok : Message
    {
        public Bind_Facebook_Account_Ok(Device Device) : base(Device)
        {
        }

        internal override short Type => 14201;

        internal override void Encode()
        {
            this.Data.AddInt(1);
        }
    }
}