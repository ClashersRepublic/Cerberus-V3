namespace CR.Servers.CoC.Packets.Messages.Server.Authentication
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Unlock_Account_OK : Message
    {
        internal Player Account;

        internal Unlock_Account_OK(Device device) : base(device)
        {
        }

        internal override short Type => 20132;

        internal override void Encode()
        {
            this.Data.AddLong(this.Account.UserId);
            this.Data.AddString(this.Account.Token);
        }
    }
}