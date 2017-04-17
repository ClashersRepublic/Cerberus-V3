using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.StreamEntry;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Commands
{
    internal class ChallangeCommand : Command
    {
        public string Message;

        public ChallangeCommand(PacketReader br)
        {
            Message = br.ReadString();
        }

        public override void Execute(Level level)
        {
          ChallangeCommand challangeCommand = this;
          try
          {
            ClientAvatar player = level.GetPlayerAvatar();
            Alliance alliance = ObjectManager.GetAlliance(player.GetAllianceId());
            ChallengeStreamEntry cm = new ChallengeStreamEntry();
            cm.SetMessage(challangeCommand.Message);
            cm.SetSenderId(player.GetId());
            cm.SetSenderName(player.GetAvatarName());
            cm.SetSenderLevel(player.GetAvatarLevel());
            ChallengeStreamEntry challengeStreamEntry = cm;
            int allianceRole = player.GetAllianceRole();
            challengeStreamEntry.SetSenderRole(allianceRole);
            challengeStreamEntry = (ChallengeStreamEntry) null;
            cm.SetId(alliance.GetChatMessages().Count + 1);
            cm.SetSenderLeagueId(player.GetLeagueId());
            alliance.AddChatMessage((Magic.Logic.StreamEntry.StreamEntry) cm);
            Magic.Logic.StreamEntry.StreamEntry s = alliance.GetChatMessages().Find((Predicate<Magic.Logic.StreamEntry.StreamEntry>)(c => c.GetStreamEntryType() == 12));
            List<AllianceMemberEntry>.Enumerator enumerator;
            if (s != null)
            {
              alliance.GetChatMessages().RemoveAll((Predicate<Magic.Logic.StreamEntry.StreamEntry>)(t => t == s));
              foreach (AllianceMemberEntry allianceMember in alliance.GetAllianceMembers())
              {
                Level player1 = ResourcesManager.GetPlayer(allianceMember.GetAvatarId(), false);
                if (player1.GetClient() != null)
                  new AllianceStreamEntryRemovedMessage(player1.GetClient(), s.GetId()).Send();
              }
              enumerator = new List<AllianceMemberEntry>.Enumerator();
            }
            foreach (AllianceMemberEntry allianceMember in alliance.GetAllianceMembers())
            {
                    Level player1 = ResourcesManager.GetPlayer(allianceMember.GetAvatarId(), false);
                    if (player1.GetClient() != null)
                    {
                        AllianceStreamEntryMessage Message = new AllianceStreamEntryMessage(player1.GetClient());
                        ChallengeStreamEntry challengeStreamEntry1 = cm;
                        Message.SetStreamEntry((Magic.Logic.StreamEntry.StreamEntry)challengeStreamEntry1);
                        Message.Send();
                    }
                }
                enumerator = new List<AllianceMemberEntry>.Enumerator();
                player = (ClientAvatar)null;
                alliance = (Alliance)null;
                cm = (ChallengeStreamEntry) null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}