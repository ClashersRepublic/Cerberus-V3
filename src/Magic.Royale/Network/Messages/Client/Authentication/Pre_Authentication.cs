using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network.Messages.Server.Authentication;

namespace Magic.ClashOfClans.Network.Messages.Client.Authentication
{
    internal class Pre_Authentication : Message
    {

        public Pre_Authentication(Device device, Reader Reader) : base(device, Reader)
        {
        }

        public string Hash;
        public int MajorVersion;
        public int MinorVersion;
        public int Protocol;
        public int KeyVersion;
        public int Unknown;
        public int DeviceType;
        public int Store;

        public override void Decode()
        {
            Protocol = Reader.ReadInt32();
            KeyVersion = Reader.ReadInt32();
            MajorVersion = Reader.ReadInt32();
            Unknown = Reader.ReadInt32();
            MinorVersion = Reader.ReadInt32();
            Hash = Reader.ReadString();
            DeviceType = Reader.ReadInt32();
            Store = Reader.ReadInt32();
        }

        public override void Process()
        {
           // if (this.Major == Convert.ToInt32(Constants.ClientVersion[0]) && this.Minor == Convert.ToInt32(Constants.ClientVersion[1]))
            {
                //if (Constants.Maintenance == null)
                {
                    // if (string.Equals(this.Hash, Fingerprint.Sha))
                    {
                        //   new Pre_Authentication_OK(this.Device).Send();
                        //}
                        //else
                        //{
                        new Authentication_Failed(Device, Reason.Patch).Send();
                    }
                }
               // else
                    //new Authentication_Failed(this.Device, Reason.Maintenance).Send();
            }
            //else
                //new Authentication_Failed(this.Device, Reason.Update).Send();
        }

    }
}