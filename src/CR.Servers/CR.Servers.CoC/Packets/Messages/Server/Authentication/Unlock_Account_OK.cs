using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Unlock_Account_OK : Message
    {
        internal Unlock_Account_OK(Device device) : base(device)
        {
        }

        internal override short Type => 20132;

        internal Player Account;

        internal override void Encode()
        {
            this.Data.AddLong(this.Account.UserId);
            this.Data.AddString(this.Account.Token);
        }
    }
}
