using System;
using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.CoC.Packets.Messages.Server.Authentication;
using CR.Servers.CoC.Packets.Messages.Server.Avatar;
using CR.Servers.CoC.Packets.Messages.Server.Friend;
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

        internal int LocaleId;


        public Authentication(Device device, Reader reader) : base(device, reader)
        {
            Device.State = State.LOGIN;
        }

        internal override void Decode()
        {
            this.HighId = Reader.ReadInt32();
            this.LowId = Reader.ReadInt32();

            this.Token = this.Reader.ReadString();

            this.Major = this.Reader.ReadInt32();
            this.Minor = this.Reader.ReadInt32();
            this.Revision = this.Reader.ReadInt32();

            this.MasterHash = Reader.ReadString();

            this.Reader.ReadString();
            this.Device.Info.UDID = this.Reader.ReadString();
            this.Device.Info.OpenUDID = this.Reader.ReadString();
            this.Device.Info.DeviceModel = this.Reader.ReadString();

            if (!this.Reader.EndOfStream)
            {
                this.LocaleId = this.Reader.ReadInt32();
                this.Device.Info.PreferredLanguage = this.Reader.ReadString();

                if (!this.Reader.EndOfStream)
                {
                    this.Device.Info.ADID = this.Reader.ReadString();

                    if (!this.Reader.EndOfStream)
                    {
                        this.Device.Info.OSVersion = this.Reader.ReadString();

                        if (!this.Reader.EndOfStream)
                        {
                            this.Device.Info.Android = this.Reader.ReadBoolean();

                            if (!this.Reader.EndOfStream)
                            {
                                this.Reader.ReadString();
                                this.Device.Info.AndroidID = this.Reader.ReadString();

                                if (!this.Reader.EndOfStream)
                                {
                                    this.Reader.ReadString();

                                    if (!this.Reader.EndOfStream)
                                    {
                                        this.Device.Info.Advertising = this.Reader.ReadBoolean();
                                        this.Reader.ReadString();

                                        if (!this.Reader.EndOfStream)
                                        {
                                            this.Device.EncryptionSeed = this.Reader.ReadInt32();
                                            if (!this.Reader.EndOfStream)
                                            {
                                                this.Reader.ReadVInt();
                                                this.Reader.ReadString();
                                                this.Reader.ReadString();

                                                if (!this.Reader.EndOfStream)
                                                {
                                                    this.Device.Info.ClientVersion =  this.Reader.ReadString().Split('.');

                                                    if (!this.Reader.EndOfStream)
                                                    {
                                                        this.Reader.ReadString(); 
                                                        this.Reader.ReadString(); 

                                                        this.Reader.ReadVInt();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal override void Process()
        {
            if (!this.CheckClient())
            {
                return;
            }

            if (Resources.Closing)
            {
                new Authentication_Failed(Device, LoginFailedReason.UpdateInProgress){Message = "The server is currently exiting and saving data, and you cannot log-in during this time. Please try again in a few minutes." }.Send();
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
            this.Device.GameMode.LoadLevel(account.Player.Level);

            this.Device.Info.Locale = this.LocaleId >= 0 ? this.LocaleId : 2000000;

            var Player = account.Player;

            if (this.Device.UseRC4)
                new SessionKey(this.Device).Send();

            new Authentication_OK(this.Device).Send();

            new Own_Home_Data(this.Device).Send();
            new Avatar_Stream(this.Device).Send();

            if (Player.InAlliance)
            {
                new Alliance_Full_Entry(this.Device) {Alliance = Player.Alliance}.Send();

                try
                {
                    new Alliance_Stream(this.Device) { Alliance = Player.Alliance }.Send();
                }
                catch (Exception Exception)
                {
                    Logging.Error(Exception.GetType(), $"Exception happend when trying to send Alliance Stream. {Exception.Message} : [{this.HighId}-{this.LowId}]" + Environment.NewLine + Exception.StackTrace);
                }

                //new Alliance_Member_State(this.Device).Send();

                this.Device.GameMode.Level.Player.Alliance.Members.Connected.TryAdd(Player.UserId, Player);
                Player.Alliance.IncrementTotalConnected();
            }

            new Friend_List(this.Device).Send();

            if (this.Device.Chat == null)
            {
                Resources.Chats.Join(this.Device);
            }
            
        }
    }
}
