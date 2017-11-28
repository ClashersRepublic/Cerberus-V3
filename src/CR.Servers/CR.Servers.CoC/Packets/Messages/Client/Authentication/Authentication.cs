using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.CoC.Packets.Messages.Server.Authentication;
using CR.Servers.CoC.Packets.Messages.Server.Avatar;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Client.Authentication
{
    internal class Authentication : Message
    {
        internal override short Type => 10101;

        internal int HighId;
        internal int LowId;

        internal string Token, MasterHash;

        internal int Major, Minor, Revision;

        public Authentication(Device device, Reader reader) : base(device, reader)
        {
            Device.State = State.LOGIN;
        }

        internal override void Decode()
        {
            HighId = Reader.ReadInt32();
            LowId = Reader.ReadInt32();

            Token = Reader.ReadString();

            Major = Reader.ReadInt32();
            Minor = Reader.ReadInt32();
            Revision = Reader.ReadInt32();

            MasterHash = Reader.ReadString();

            Reader.ReadString();


            Device.Info.UDID = Reader.ReadString();
            Device.Info.OpenUDID = Reader.ReadString();
            Device.Info.DeviceModel = Reader.ReadString();

            Device.Info.LocaleData = Reader.ReadData<LocaleData>(); 

            Device.Info.PreferredLanguage = Reader.ReadString();
            Device.Info.ADID = Reader.ReadString();
            Device.Info.OSVersion = Reader.ReadString();

            Device.Info.Android = Reader.ReadBoolean();

            Reader.ReadString();

            Device.Info.AndroidID = Reader.ReadString();

            Reader.ReadString();

            Device.Info.Advertising = Reader.ReadBoolean();

            Reader.ReadString();

            Device.Seed = Reader.ReadUInt32();

            Reader.ReadByte();
            Reader.ReadString();
            Reader.ReadString();

            Device.Info.ClientVersion = Reader.ReadString().Split('.');
        }

        internal void Process2()
        {
            ShowValues();
            Device.Info.ShowValues();
            new SessionKey(Device).Send();
            new Authentication_Failed(Device, LoginFailedReason.Locked).Send();
            
        }

        internal override void Process()
        {
            if (!this.CheckClient())
            {
                return;
            }

            if (this.HighId == 0 && this.LowId == 0 && this.Token == null)
            {
                var Account = Resources.Accounts.CreateAccount();

                if (Account.Player != null && Account.Home != null)
                {
                    if (Account.Player.Locked)
                        new Authentication_Failed(Device, LoginFailedReason.Locked).Send();
                    else
                        this.Login(Account);
                }
                else
                {
                    new Authentication_Failed(this.Device, LoginFailedReason.Maintenance).Send();
                }
            }
            else
            {
                var Account = Resources.Accounts.LoadAccount(this.HighId, this.LowId);

                if (Account?.Player != null && Account?.Home != null)
                {
                    if (string.Equals(this.Token, Account.Player.Token))
                    {
                        if (!Account.Player.Locked)
                        {
                            if (!Account.Player.Banned)
                            {
                                this.Login(Account);
                            }
                            else
                            {
                                new Authentication_Failed(this.Device, LoginFailedReason.Banned).Send();
                            }
                        }
                        else
                        {
                            new Authentication_Failed(this.Device, LoginFailedReason.Locked).Send();
                        }
                    }
                    else
                    {
                        new Authentication_Failed(this.Device, LoginFailedReason.Locked).Send();
                    }
                }
                else
                {
                    new Authentication_Failed(this.Device, LoginFailedReason.Locked).Send();
                }
            }
        }

        internal bool CheckClient()
        {
            if (this.Valid())
            {
                if (this.Device.Info.ClientVersion[0] == Settings.ClientMajorVersion && this.Device.Info.ClientVersion[1] == Settings.ClientMinorVersion)
                {
                    //TODO: Maintenance

                    if (!string.IsNullOrEmpty(Settings.PatchServer))
                    {
                        if (!string.IsNullOrEmpty(Fingerprint.Json) && !string.Equals(MasterHash, Fingerprint.Sha))
                        {
                            new Authentication_Failed(Device, LoginFailedReason.Patch).Send();
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    new Authentication_Failed(this.Device, LoginFailedReason.Update).Send();
                }
            }
            else
            {
                //Buggy?
                Resources.Gateway.Disconnect(this.Device.Token.Args);
            }

            return false;
        }

        internal bool Valid()
        {
            if (this.HighId >= 0)
            {
                if (this.LowId >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        internal void Login(Account account)
        {
            this.Device.Account = account;
            this.Device.GameMode.LoadHomeState(account.Home, account.Player);
            var Player = account.Player;
            Player.VerifyAlliance();

            if (this.Device.ReceiveDecrypter.IsRC4)
                new SessionKey(this.Device).Send();

            new Authentication_OK(this.Device).Send();
            new Own_Home_Data(this.Device).Send();
            new Avatar_Stream(this.Device).Send();

            if (Player.InAlliance)
            {
                new Alliance_Full_Entry(this.Device) {Alliance = Player.Alliance}.Send();
                new Alliance_Stream(this.Device) {Alliance = Player.Alliance}.Send();
                this.Device.GameMode.Level.Player.Alliance.Members.Connected.TryAdd(Player.UserId, Player);
            }
        }
    }
}
