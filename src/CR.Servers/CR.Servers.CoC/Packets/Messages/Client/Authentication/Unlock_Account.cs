using System;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Authentication;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class Unlock_Account : Message
    {
        internal override short Type => 10121;

        internal long UserID;
        internal string UserToken;
        internal string UnlockCode;
        internal string UserPassword;


        public Unlock_Account(Device device, Reader reader) : base(device, reader)
        {
            this.UserPassword = this.Device.Account?.Player != null ? this.Device.Account.Player.Password : string.Empty;
        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();
            this.UserToken = this.Reader.ReadString();

            this.UnlockCode = this.Reader.ReadString();
        }

        internal override void Process()
        {
            if (this.UnlockCode.Length != 12 || string.IsNullOrEmpty(this.UnlockCode))
            {
                //Remove Device
                return;
            }

            if (this.UnlockCode[0] == '/')
            {
                int n = 0;
                if (int.TryParse(this.UnlockCode.Substring(1), out n))
                {
                    if (n == 0)
                    {
                        //Send new player
                        new Unlock_Account_OK(this.Device) { Account = Resources.Accounts.CreateAccount().Player }.Send();
                        return;
                    }

                    int HighId = Convert.ToInt32(n >> 32);
                    int LowId = (int) n;

                    var Account = Resources.Accounts.LoadAccount(HighId, LowId);
                    if (Account != null)
                    {
                        Account.Player.Locked = true;
                        new Unlock_Account_OK(this.Device) { Account = Account.Player }.Send();
                    }
                    else
                    {
                        new Unlock_Account_Failed(this.Device) { Reason = UnlockAccountReason.UnlockError }.Send();
                    }

                }
                else
                {
                    new Unlock_Account_Failed(this.Device) { Reason = UnlockAccountReason.UnlockError }.Send();
                }
            }
            if (string.Equals(this.UnlockCode, this.UserPassword, StringComparison.CurrentCultureIgnoreCase))
            {
                this.Device.Account.Player.Locked = false;
                new Unlock_Account_OK(this.Device) { Account = this.Device.Account.Player }.Send();
            }
            else
            {
                new Unlock_Account_Failed(this.Device) { Reason = UnlockAccountReason.Default }.Send();

            }
        }
    }
}
