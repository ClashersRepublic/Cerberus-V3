﻿using System;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.AvatarStreamEntries;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class SendAllianceMailCommand : Command
    {
        internal string m_vMailContent;

        public SendAllianceMailCommand(PacketReader br)
        {
            m_vMailContent = br.ReadString();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            SendAllianceMailCommand allianceMailCommand = this;
            try
            {
                ClientAvatar avatar = level.Avatar;
                long allianceId = avatar.GetAllianceId();
                if (allianceId > 0L)
                {
                    Alliance alliance = ObjectManager.GetAlliance(allianceId);
                    if (alliance != null)
                    {
                        AllianceMailStreamEntry allianceMailStreamEntry1 = new AllianceMailStreamEntry();
                        allianceMailStreamEntry1.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        allianceMailStreamEntry1.SetAvatar(avatar);
                        allianceMailStreamEntry1.SetIsNew((byte)2);
                        allianceMailStreamEntry1.SetSenderId(avatar.Id);
                        allianceMailStreamEntry1.SetAllianceId(allianceId);
                        allianceMailStreamEntry1.SetAllianceBadgeData(alliance.AllianceBadgeData);
                        allianceMailStreamEntry1.SetAllianceName(alliance.AllianceName);
                        allianceMailStreamEntry1.SetMessage(m_vMailContent);
                        foreach (Level onlinePlayer in ResourcesManager.OnlinePlayers)
                        {
                            if (onlinePlayer.Avatar.GetAllianceId() == allianceId)
                            {
                                AvatarStreamEntryMessage Message = new AvatarStreamEntryMessage(onlinePlayer.Client);
                                AllianceMailStreamEntry allianceMailStreamEntry2 = allianceMailStreamEntry1;
                                Message.SetAvatarStreamEntry((Magic.Logic.AvatarStreamEntries.AvatarStreamEntry)allianceMailStreamEntry2);
                                Message.Send();
                            }
                        }
                    }
                }
                avatar = (ClientAvatar) null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}