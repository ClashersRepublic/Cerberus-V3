using System.Linq;
using Magic.Files;
using Magic.Royale.Core;
using Magic.Royale.Core.Crypto;
using Magic.Royale.Core.Settings;
using Magic.Royale.Extensions.Binary;
using Magic.Royale.Extensions.Blake2B;
using Magic.Royale.Extensions.Sodium;
using Magic.Royale.Logic.Enums;
using Magic.Royale.Network.Messages.Server;
using Magic.Royale.Network.Messages.Server.Authentication;

namespace Magic.Royale.Network.Messages.Client.Authentication
{
    internal class Authentication : Message
    {
        public Authentication(Device device, Reader reader) : base(device, reader)
        {
            Device.State = State.LOGIN;
        }

        internal long UserId;

        internal string Token, MasterHash, Language, UDID;

        public override void Decrypt()
        {
            var buffer = Reader.ReadBytes(Length);
            Device.PublicKey = buffer.Take(32).ToArray();

            var Blake = new Blake2BHasher();

            Blake.Update(Device.PublicKey);
            Blake.Update(Key.PublicKey);

            Device.RNonce = Blake.Finish();

            buffer = Sodium.Decrypt(buffer.Skip(32).ToArray(), Device.RNonce, Key.PrivateKey, Device.PublicKey);
            Device.SNonce = buffer.Skip(24).Take(24).ToArray();
            Reader = new Reader(buffer.Skip(48).ToArray());

            Length = (ushort) buffer.Length;
        }

        public override void Decode()
        {
            UserId = Reader.ReadInt64();

            Token = Reader.ReadString();

            Device.Major = Reader.ReadVInt();
            Device.Minor = Reader.ReadVInt();
            Device.Revision = Reader.ReadVInt();

            MasterHash = Reader.ReadString();

            UDID = Reader.ReadString();

            Device.OpenUDID = Reader.ReadString();
            Device.MACAddress = Reader.ReadString();
            Device.Model = Reader.ReadString();
            Device.AdvertiseID = Reader.ReadString();

            Device.OSVersion = Reader.ReadString();

            Reader.ReadByte();

            Reader.Seek(4);

            Device.AndroidID = Reader.ReadString();
            Language = Reader.ReadString();

            Reader.ReadByte();
            Reader.ReadByte();

            Reader.ReadString();

            Reader.ReadByte();

            Reader.Seek(4);

            Reader.ReadByte();

            Reader.Seek(17);
        }

        public override void Process()
        {
            /*if (!string.IsNullOrEmpty(Constants.PatchServer))
                if (!string.IsNullOrEmpty(Fingerprint.Json) && !string.Equals(MasterHash, Fingerprint.Sha))
                {
                    new Authentication_Failed(Device, Reason.Patch).Send();
                    return;
                }
            */
            CheckClient();
        }

        private void LogUser()
        {
            ResourcesManager.LogPlayerIn(Device.Player);
            Device.Player.Device = Device;

            new Authentication_OK(Device).Send();

            new Own_Home_Data(Device).Send();
        }

        private void CheckClient()
        {
            if (UserId == 0)
                if (Token == null)
                {
                    Device.Player = ObjectManager.CreateLevel(0);

                    if (Device.Player != null)
                        if (Device.Player.Locked)
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
                        if (string.Equals(Token, Device.Player.Token))
                            if (Device.Player.Locked)
                            {
                                new Authentication_Failed(Device, Reason.Locked).Send();
                            }
                            else if (Device.Player.Banned)
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
