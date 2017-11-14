using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Unlock_Account_Failed : Message
    {
        
        internal Unlock_Account_Failed(Device device) : base(device)
        {
            this.Version = 1;
        }

        internal override short Type => 20133;

        internal UnlockAccountReason Reason;

        internal override void Encode()
        {
            this.Data.AddInt((int)this.Reason);
        }
    }
}
