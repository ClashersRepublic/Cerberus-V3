using System;
using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AvatarProfileMessage : Message
    {
        public Level Level;

        public AvatarProfileMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24334);
        }

        public override void Encode()
        {
            var data = new List<byte>();

            data.AddRange(Level.GetPlayerAvatar().Encode());
            data.AddCompressedString(Level.SaveToJson());

            data.AddInt32(0); //Donated
            data.AddInt32(0); //Received
            data.AddInt32(0); //War Cooldown

            data.AddInt32(0); //Unknown
            data.Add(0); //Unknown

            Encrypt(data.ToArray());
        }
    }
}
