﻿using System;
using System.Configuration;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Core.Settings;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class HandshakeRequest : Message
    {

        public HandshakeRequest(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public string Hash;
        public int MajorVersion;
        public int MinorVersion;
        public int Protocol;
        public int KeyVersion;
        public int Unknown;
        public int Device;
        public int Store;

        public override void Decode()
        {
            using (var reader = new PacketReader(new MemoryStream(Data)))
            {
                Protocol = reader.ReadInt32();
                KeyVersion = reader.ReadInt32();
                MajorVersion = reader.ReadInt32();
                Unknown = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                Hash = reader.ReadString();
                Device = reader.ReadInt32();
                Store = reader.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            if (Constants.IsRc4)
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["patchingServer"]))
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL(ConfigurationManager.AppSettings["UpdateUrl"]);
                    p.Send();
                }
                else
                    throw new NullReferenceException("Patching server cannot be nulled in RC4");
            }
            else
                new HandshakeSuccess(Client, this).Send();
        }

    }
}