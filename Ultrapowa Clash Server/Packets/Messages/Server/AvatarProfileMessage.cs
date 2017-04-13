using System;
using System.Collections.Generic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Messages.Server
{
    internal class AvatarProfileMessage : Message
    {
        internal Level m_vLevel;

        public AvatarProfileMessage(PacketProcessing.Client client)
            : base(client)
        {
            SetMessageType(24334);
        }

        public override void Encode()
        {
            try
            {
                var pack = new List<byte>();
                var ch = new ClientHome(m_vLevel.GetPlayerAvatar().GetId());
                ch.SetHomeJSON(m_vLevel.SaveToJSON());

                pack.AddRange(m_vLevel.GetPlayerAvatar().Encode());
                pack.AddCompressedString(ch.GetHomeJSON());

                pack.AddInt32(0); //Donated
                pack.AddInt32(0); //Received
                pack.AddInt32(0); //War Cooldown

                pack.AddInt32(0); //Unknown
                pack.Add(0); //Unknown

                ch = (ClientHome)null;


                Encrypt(pack.ToArray());
            }
            catch (Exception ex)
            {
            }
        }

        public void SetLevel(Level level)
        {
            m_vLevel = level;
        }
    }
}
