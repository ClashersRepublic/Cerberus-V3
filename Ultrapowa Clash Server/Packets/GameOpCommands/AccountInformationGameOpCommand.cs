﻿

using System;
using UCS.Core;
using UCS.Core.Network;
using UCS.Logic;
using UCS.Logic.AvatarStreamEntry;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.GameOpCommands
{
    internal class AccountInformationGameOpCommand : GameOpCommand
    {
        private readonly string[] m_vArgs;
        private string Message;
        public AccountInformationGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(5);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 2)
                {
                    try
                    {
                        long id = Convert.ToInt64(m_vArgs[1]);
                        Level l = ResourcesManager.GetPlayer(id);
                        if (l != null)
                        {
                            ClientAvatar acc = l.GetPlayerAvatar();
                            Message = "Player Info : \n\n" + "ID = " + id + "\nName = " + acc.GetAvatarName() + "\nCreation Date : " + acc.GetAccountCreationDate() + "\nRegion : " + acc.GetUserRegion() + "\nIP Address : " + l.GetIPAddress();
                            if (acc.GetAllianceId() != 0)
                            {
                                Message = Message + "\nClan Name : " + ObjectManager.GetAlliance(acc.GetAllianceId()).GetAllianceName();
                                switch (acc.GetAllianceRole())
                                {
                                    case 1:
                                        Message = Message + "\nClan Role : Member";
                                        break;

                                    case 2:
                                        Message = Message + "\nClan Role : Leader";
                                        break;

                                    case 3:
                                        Message = Message + "\nClan Role : Elder";
                                        break;

                                    case 4:
                                        Message = Message + "\nClan Role : Co-Leader";
                                        break;

                                    default:
                                        Message = Message + "\nClan Role : Unknown";
                                        break;
                                }
                            }
                            Message = Message + "\nLevel : " + acc.GetAvatarLevel() + "\nTrophy : " + acc.GetScore() + "\nTown Hall Level : " + (acc.GetTownHallLevel() + 1)  + "\nAlliance Castle Level : " + (acc.GetAllianceCastleLevel() + 1);

                            var avatar = level.GetPlayerAvatar();
                            AllianceMailStreamEntry mail = new AllianceMailStreamEntry();
                            mail.SetSenderId(avatar.GetId());
                            mail.SetSenderAvatarId(avatar.GetId());
                            mail.SetSenderName(avatar.GetAvatarName());
                            mail.SetIsNew(2);
                            mail.SetAllianceId(0);
                            mail.SetAllianceBadgeData(1526735450);
                            mail.SetAllianceName("UCS Server Information");
                            mail.SetMessage(Message);
                            mail.SetSenderLevel(avatar.GetAvatarLevel());
                            mail.SetSenderLeagueId(avatar.GetLeagueId());

                            AvatarStreamEntryMessage p = new AvatarStreamEntryMessage(level.GetClient());
                            p.SetAvatarStreamEntry(mail);
                            p.Send();
                        }
                    }
                    catch (Exception)
                    {
                        GlobalChatLineMessage c = new GlobalChatLineMessage(level.GetClient());
                        c.SetChatMessage("Command Failed, Wrong Format Or User Doesn't Exist (/accinfo id).");
                        c.SetPlayerId(level.GetPlayerAvatar().GetId());
                        c.SetLeagueId(22);
                        c.SetPlayerName("Clash of Magic");
                        c.Send();
                        return;
                    }
                }
                else
                {
                    GlobalChatLineMessage b = new GlobalChatLineMessage(level.GetClient());
                    b.SetChatMessage("Command Failed, Wrong Format (/accinfo id).");
                    b.SetPlayerId(level.GetPlayerAvatar().GetId());
                    b.SetLeagueId(22);
                    b.SetPlayerName("Clash of Magic");
                    b.Send();
                }
            }
        }
    }
}


