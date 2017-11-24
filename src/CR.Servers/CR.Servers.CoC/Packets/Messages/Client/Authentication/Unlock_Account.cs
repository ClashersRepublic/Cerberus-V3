using System;
using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Authentication;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class Unlock_Account : Message
    {
        internal override short Type => 10121;

        internal int HighId;
        internal int LowId;
        internal string UserToken;
        internal string UnlockCode;

        public Unlock_Account(Device device, Reader reader) : base(device, reader)
        {
            Device.State = State.UNLOCK_ACC;
        }

        internal override void Decode()
        {
            this.HighId = this.Reader.ReadInt32();
            this.LowId = this.Reader.ReadInt32();
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
                        new Unlock_Account_OK(this.Device) {Account = Resources.Accounts.CreateAccount().Player}.Send();
                        return;
                    }

                    int HighId = 0;
                    int LowId = 0;

                    this.ToHighAndLow(n, ref HighId, ref LowId);
                    var Account = Resources.Accounts.LoadAccount(HighId, LowId);
                    if (Account != null)
                    {
                        Account.Player.Locked = true;
                        new Unlock_Account_OK(this.Device) {Account = Account.Player}.Send();
                    }
                    else
                    {
                        new Unlock_Account_Failed(this.Device) {Reason = UnlockAccountReason.UnlockError}.Send();
                    }

                }
                else
                {
                    new Unlock_Account_Failed(this.Device) {Reason = UnlockAccountReason.UnlockError}.Send();
                }
            }
            else
            {
                var Account = Resources.Accounts.LoadPlayerAsync(this.HighId, this.LowId).Result;
                if (Account != null)
                {
                    if (string.Equals(this.UnlockCode, Account.Password, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Account.Locked = false;
                        new Unlock_Account_OK(this.Device) {Account = Account}.Send();
                    }
                    else
                    {
                        new Unlock_Account_Failed(this.Device) {Reason = UnlockAccountReason.Default}.Send();

                    }
                }
                else
                    new Unlock_Account_Failed(this.Device) { Reason = UnlockAccountReason.UnlockError }.Send();
            }
        }

        internal void ToHighAndLow(long l, ref int High, ref int Low)
        {
            var bytes = new Reader(BitConverter.GetBytes(l).Reverse().ToArray());

            High = bytes.ReadInt32();
            Low = bytes.ReadInt32();
        }
    }
}
