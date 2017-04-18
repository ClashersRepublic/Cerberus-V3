using System;
using Magic.Core.Network;
using Magic.Logic;
using Magic.Logic.AvatarStreamEntries;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.GameOpCommands
{
    internal class GetIdGameopCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public GetIdGameopCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 1)
                {
                    var avatar = level.Avatar;
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetSenderId(avatar.Id);
                    mail.SetSenderAvatarId(avatar.Id);
                    mail.SetSenderName(avatar.GetAvatarName());
                    mail.SetIsNew(0);
                    mail.SetAllianceId(1);
                    mail.SetAllianceBadgeData(1526735450);
                    mail.SetAllianceName("UCS Information");
                    mail.SetMessage("Your Player ID: " + level.Avatar.Id);
                    mail.SetSenderLevel(avatar.GetAvatarLevel());
                    mail.SetSenderLeagueId(avatar.GetLeagueId());

                    var p = new AvatarStreamEntryMessage(level.Client);
                    p.SetAvatarStreamEntry(mail);
                    p.Send();
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
