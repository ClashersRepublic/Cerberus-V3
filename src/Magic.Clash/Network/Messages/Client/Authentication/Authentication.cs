using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.ClashOfClans.Network.Messages.Server.Authentication;
using Magic.ClashOfClans.Network.Messages.Server.Clans;
using Magic.Files;
using Avatar_Stream = Magic.ClashOfClans.Network.Messages.Server.Avatar_Stream;

namespace Magic.ClashOfClans.Network.Messages.Client.Authentication
{
    internal class Authentication : Message
    {
        public Authentication(Device device, Reader reader) : base(device, reader)
        {
            Device.State = State.LOGIN;
        }

        internal long UserId;

        internal string Token, MasterHash, Language, Udid;

        internal int Major, Minor, Revision, Locale;
        internal string[] ClientVersion;

        public override void Decode()
        {
            UserId = Reader.ReadInt64();

            Token = Reader.ReadString();

            Major = Reader.ReadInt32();
            Minor = Reader.ReadInt32();
            Revision = Reader.ReadInt32();

            MasterHash = Reader.ReadString();

            Reader.ReadString();

            Device.AndroidID = Reader.ReadString();
            Device.MACAddress = Reader.ReadString();
            Device.Model = Reader.ReadString();

            Locale = Reader.ReadInt32(); // 2000001

            Language = Reader.ReadString();
            Device.OpenUDID = Reader.ReadString();
            Device.OSVersion = Reader.ReadString();

            Device.Android = Reader.ReadBoolean();

            Reader.ReadString();

            Device.AndroidID = Reader.ReadString();

            Reader.ReadString();

            Device.Advertising = Reader.ReadBoolean();

            Reader.ReadString();

            Device.ClientSeed = Reader.ReadUInt32();

            Reader.ReadByte();
            Reader.ReadString();
            Reader.ReadString();

            ClientVersion = Reader.ReadString().Split('.');
        }

        public override void Process()
        {
            if (Constants.IsRc4)
                new SessionKey(Device).Send();

            if (ClientVersion[0] != Constants.ClientVersion[0] || ClientVersion[1] != Constants.ClientVersion[1])
            {
                new Authentication_Failed(Device, Reason.Update).Send();
                return;
            }

            if (!string.IsNullOrEmpty(Constants.PatchServer))
                if (!string.IsNullOrEmpty(Fingerprint.Json) && !string.Equals(MasterHash, Fingerprint.Sha))
                {
                    new Authentication_Failed(Device, Reason.Patch).Send();
                    return;
                }

            CheckClient();
        }

        private void LogUser()
        {
            ResourcesManager.LogPlayerIn(Device.Player);
            Device.Player.Device = Device;

            new Authentication_OK(Device).Send();
            new Own_Home_Data(Device).Send();
            new Avatar_Stream(Device).Send();
            new Bookmark(Device).Send();

            if (Device.Player.Avatar.ClanId > 0)
            {
                var Alliance = ObjectManager.GetAlliance(Device.Player.Avatar.ClanId);

                if (Alliance != null)
                {
                    Device.Player.Avatar.Alliance_Level = Alliance.Level;

                    new Alliance_Full_Entry(Device) {Clan = Alliance}.Send();

                    if (Alliance.Chats != null)
                        new Alliance_All_Stream_Entry(Device, Alliance).Send();
                }
                else
                {
                    Device.Player.Avatar.ClanId = 0;
                }
            }
        }

        private void CheckClient()
        {
            if (UserId == 0)
                if (Token == null)
                {
                    Device.Player = ObjectManager.CreateLevel(0);

                    if (Device.Player != null)
                        if (Device.Player.Avatar.Locked)
                            new Authentication_Failed(Device, Reason.Locked).Send();
                        else
                            LogUser();
                    else
                        new Authentication_Failed(Device, Reason.Pause).Send();
                }
                else
                {
                    new Authentication_Failed(Device, (Reason) 2).Send();
                }
            else if (UserId > 0)
                if (Token == null)
                {
                    new Authentication_Failed(Device, (Reason) 2).Send();
                }
                else
                {
                    Device.Player = DatabaseManager.GetLevel(UserId);
                    if (Device.Player != null)
                    {
                        if (string.Equals(Token, Device.Player.Avatar.Token))
                            if (Device.Player.Avatar.Locked)
                            {
                                new Authentication_Failed(Device, Reason.Locked).Send();
                            }
                            else if (Device.Player.Avatar.Banned)
                            {
                            }
                            else
                            {
                                LogUser();
                            }
                        else
                            new Authentication_Failed(Device, Reason.Locked).Send();
                    }
                    else
                    {
                        Device.Player = ObjectManager.CreateLevel(UserId, Token);
                        LogUser();
                    }
                }
        }
    }
}
