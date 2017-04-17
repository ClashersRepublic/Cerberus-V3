using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using Magic.Core;
using Magic.Core.Network;
using Magic.Core.Settings;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.AvatarStreamEntry;
using Magic.PacketProcessing.Messages.Server;
using Magic.Packets.Messages.Server;
using static Magic.PacketProcessing.Client;

namespace Magic.PacketProcessing.Messages.Client
{
    // Packet 10101
    internal class LoginMessage : Message
    {
        public LoginMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {

        }

        public string AdvertisingGUID;
        public string AndroidDeviceID;
        public string ClientVersion;
        public string DeviceModel;
        public string FacebookDistributionID;
        public string Region;
        public string MacAddress;
        public string MasterHash;
        public string OpenUDID;
        public string OSVersion;
        public string UserToken;
        public string VendorGUID;
        public int ContentVersion;
        public int LocaleKey;
        public int MajorVersion;
        public int MinorVersion;
        public uint Seed;
        public bool IsAdvertisingTrackingEnabled;
        public bool Android;
        public long UserID;

        public Level level;

        public override void Decode()
        {
            if (Client.State >= ClientState.Login)
            {
                try
                {
                    using (var reader = new PacketReader(new MemoryStream(Data)))
                    {
                        UserID = reader.ReadInt64();
                        UserToken = reader.ReadString();
                        MajorVersion = reader.ReadInt32();
                        ContentVersion = reader.ReadInt32();
                        MinorVersion = reader.ReadInt32();
                        MasterHash = reader.ReadString();
                        reader.ReadString();
                        OpenUDID = reader.ReadString();
                        MacAddress = reader.ReadString();
                        DeviceModel = reader.ReadString();
                        LocaleKey = reader.ReadInt32();
                        Region = reader.ReadString();
                        AdvertisingGUID = reader.ReadString();
                        OSVersion = reader.ReadString();
                        Android = reader.ReadBoolean();
                        reader.ReadString();
                        AndroidDeviceID = reader.ReadString();
                        FacebookDistributionID = reader.ReadString();
                        IsAdvertisingTrackingEnabled = reader.ReadBoolean();
                        VendorGUID = reader.ReadString();
                        Seed = reader.ReadUInt32();
                        reader.ReadByte();
                        reader.ReadString();
                        reader.ReadString();
                        ClientVersion = reader.ReadString();
                    }
                }
                catch
                {
                    Client.State = ClientState.Exception;
                    throw;
                }
            }
        }

        public override void Process(Level a)
        {
            if (Client.State == ClientState.Login)
            {
                if (Constants.IsRc4)
                {
                    Client.ClientSeed = Seed;
                    new RC4SessionKey(Client).Send();
                }

                //if (ParserThread.GetMaintenanceMode() == true)
                //{
                //    var p = new LoginFailedMessage(Client);
                //    p.SetErrorCode(10);
                //    p.RemainingTime(ParserThread.GetMaintenanceTime());
                //    p.SetMessageVersion(8);
                //    p.Send();
                //    return;
                //}

                //if (Constants.IsPremiumServer == false)
                //{
                //    if (ResourcesManager.GetOnlinePlayers().Count >= 100)
                //    {
                //        var p = new LoginFailedMessage(Client);
                //        p.SetErrorCode(11);
                //        p.SetReason("Clash of Magic");
                //        p.Send();
                //        return;
                //    }
                //}

                int time = Convert.ToInt32(ConfigurationManager.AppSettings["maintenanceTimeleft"]);
                if (time != 0)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(10);
                    p.RemainingTime(time);
                    p.MessageVersion = 8;
                    p.Send();
                    return;
                }

                if (ConfigurationManager.AppSettings["CustomMaintenance"] != string.Empty)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(10);
                    p.SetReason(ConfigurationManager.AppSettings["CustomMaintenance"]);
                    p.Send();
                    return;
                }

                var cv2 = ConfigurationManager.AppSettings["ClientVersion"].Split('.');
                var cv = ClientVersion.Split('.');
                if (cv[0] != cv2[0] || cv[1] != cv2[1])
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(8);
                    p.SetUpdateURL(Convert.ToString(ConfigurationManager.AppSettings["UpdateUrl"]));
                    p.Send();
                    return;
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]) &&
                    MasterHash != ObjectManager.FingerPrint.sha)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL(ConfigurationManager.AppSettings["UpdateUrl"]);
                    p.Send();
                    return;
                }

                CheckClient();
            }
        }

        private void LogUser()
        {
            ResourcesManager.LogPlayerIn(level, Client);
            level.Tick();

            var loginOk = new LoginOkMessage(Client);
            var avatar = level.GetPlayerAvatar();
            loginOk.SetAccountId(avatar.GetId());
            loginOk.SetPassToken(avatar.GetUserToken());
            loginOk.SetServerMajorVersion(MajorVersion);
            loginOk.SetServerBuild(MinorVersion);
            loginOk.SetContentVersion(ContentVersion);
            loginOk.SetServerEnvironment("prod");
            loginOk.SetDaysSinceStartedPlaying(0);
            loginOk.SetServerTime(Math.Round(level.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000).ToString(CultureInfo.InvariantCulture));
            loginOk.SetAccountCreatedDate(avatar.GetAccountCreationDate().ToString());
            loginOk.SetStartupCooldownSeconds(0);
            loginOk.SetCountryCode(avatar.GetUserRegion() ?? "EN");
            loginOk.Send();

            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            if (ResourcesManager.IsPlayerOnline(level))
            {
                var mail = new AllianceMailStreamEntry();
                mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                mail.SetSenderId(0);
                mail.SetSenderAvatarId(0);
                mail.SetSenderName("Clash of Magic Admin");
                mail.SetIsNew(2);
                mail.SetAllianceId(0);
                mail.SetSenderLeagueId(22);
                mail.SetAllianceBadgeData(1526735450);
                mail.SetAllianceName("Clash of Magic Admin");
                mail.SetMessage(ConfigurationManager.AppSettings["AdminMessage"]);
                mail.SetSenderLevel(500);
                var p = new AvatarStreamEntryMessage(level.GetClient());
                p.SetAvatarStreamEntry(mail);
                p.Send();
            }

            new OwnHomeDataMessage(Client, level).Send(); // THIS MESSAGE MUST BE SENT FIRST !!!
            new AvatarStreamMessage(Client).Send();

            if (alliance != null)
            {
                new AllianceFullEntryMessage(Client, alliance).Send();
                new AllianceStreamMessage(Client, alliance).Send();
                new AllianceWarHistoryMessage(Client, alliance).Send();
                //PacketManager.ProcessOutgoingPacket (new AllianceWarMapDataMessage(Client)); //Don't activate it (not done!)
            }

            new BookmarkMessage(Client).Send();
        }

        private void CheckClient()
        {
            if (UserID == 0)
            {
                if (UserToken == null)
                {
                    NewUser();
                    return;
                }
                else
                {
                    var loginFailed = GetLoginFailedMessage(1);
                    loginFailed.Send();
                    return;
                }
            }
            else
            {
                if (UserToken == null)
                {
                    var loginFailed = GetLoginFailedMessage(2);
                    loginFailed.Send();
                    return;
                }
                else
                {
                    // Try to get player from memory then DB.
                    level = ResourcesManager.GetPlayer(UserID, true);

                    var avatar = default(ClientAvatar);
                    // If level does not exists we create a new one with the specified
                    // UserId and UserToken.
                    if (level == null)
                    {
                        level = ObjectManager.CreateLevel(UserID, UserToken);
                        avatar = level.GetPlayerAvatar();
                        avatar.SetRegion(Region);
                    }
                    else
                    {
                        avatar = level.GetPlayerAvatar();
                    }

                    // Check avatar/client password if matches user id.
                    if (avatar.GetUserToken() != UserToken)
                    {
                        var loginFailed = GetLoginFailedMessage(3);
                        loginFailed.Send();
                    }
                    else
                    {
                        LogUser();
                    }
                }
            }
        }

        private void NewUser()
        {
            level = ObjectManager.CreateLevel(0, null);
            if (string.IsNullOrEmpty(UserToken))
            {
                byte[] tokenSeed = new byte[20];
                new Random().NextBytes(tokenSeed);
                using (SHA1 sha = new SHA1CryptoServiceProvider())
                    UserToken = BitConverter.ToString(sha.ComputeHash(tokenSeed)).Replace("-", string.Empty);
            }

            level.GetPlayerAvatar().SetRegion(Region.ToUpper());
            level.GetPlayerAvatar().SetToken(UserToken);
            level.GetPlayerAvatar().InitializeAccountCreationDate();
            level.GetPlayerAvatar().SetAndroid(Android);

            DatabaseManager.Instance.Save(level);
            LogUser();
        }

        private LoginFailedMessage GetCleanUpLoginFailedMessage()
        {
            var message = new LoginFailedMessage(Client);
            message.SetErrorCode(6);
            message.SetReason("We have detected an issue with your ID. Please clear your app data to continue playing! \n\nSettings -> Application Manager -> Clear App Data\n\nFor more informations, please check our official Website.\n\nhttps://www.clashofmagic.net/");
            return message;
        }

        private LoginFailedMessage GetLoginFailedMessage(int errCode)
        {
            var message = new LoginFailedMessage(Client);
            message.SetErrorCode(6);
            message.SetReason($"CoM{errCode}\r\nWe have detected an issue with you ID, clean app data to continue.");
            return message;
        }
    }
}
