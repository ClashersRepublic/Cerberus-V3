using System;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.AvatarStreamEntry;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing.Messages.Server;
using UCS.PacketProcessing.Commands.Server;
using System.Threading.Tasks;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class KickAllianceMemberCommand : Command
    {
        public KickAllianceMemberCommand(PacketReader br)
        {
            m_vAvatarId = br.ReadInt64WithEndian();
            br.ReadByte();
            m_vMessage = br.ReadScString();
            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            var targetAccount = ResourcesManager.GetPlayer(m_vAvatarId);
            if (targetAccount != null)
            {
                var targetAvatar = targetAccount.GetPlayerAvatar();
                var targetAllianceId = targetAvatar.GetAllianceId();
                var requesterAvatar = level.GetPlayerAvatar();
                var requesterAllianceId = requesterAvatar.GetAllianceId();
                if (requesterAllianceId > 0 && targetAllianceId == requesterAllianceId)
                {
                    var alliance = ObjectManager.GetAlliance(requesterAllianceId);
                    var requesterMember = alliance.GetAllianceMember(requesterAvatar.GetId());
                    var targetMember = alliance.GetAllianceMember(m_vAvatarId);
                    if (targetMember.HasLowerRoleThan(requesterMember.GetRole()))
                    {
                        targetAvatar.SetAllianceId(0);
                        alliance.RemoveMember(m_vAvatarId);
                        if (ResourcesManager.IsPlayerOnline(targetAccount))
                        {
                            var leaveAllianceCommand = new LeavedAllianceCommand();
                            leaveAllianceCommand.SetAlliance(alliance);
                            leaveAllianceCommand.SetReason(2); //Kick
                            var availableServerCommandMessage = new AvailableServerCommandMessage(targetAccount.GetClient());
                            availableServerCommandMessage.SetCommandId(2);
                            availableServerCommandMessage.SetCommand(leaveAllianceCommand);
                            availableServerCommandMessage.Send();

                            var kickOutStreamEntry = new AllianceKickOutStreamEntry();
                            kickOutStreamEntry.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            kickOutStreamEntry.SetAvatar(requesterAvatar);
                            kickOutStreamEntry.SetIsNew(0);
                            kickOutStreamEntry.SetAllianceId(alliance.GetAllianceId());
                            kickOutStreamEntry.SetAllianceBadgeData(alliance.GetAllianceBadgeData());
                            kickOutStreamEntry.SetAllianceName(alliance.GetAllianceName());
                            kickOutStreamEntry.SetMessage(m_vMessage);

                            var p = new AvatarStreamEntryMessage(targetAccount.GetClient());
                            p.SetAvatarStreamEntry(kickOutStreamEntry);
                            p.Send();
                        }

                        var eventStreamEntry = new AllianceEventStreamEntry();
                        eventStreamEntry.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        eventStreamEntry.SetSender(targetAvatar);
                        eventStreamEntry.SetEventType(1);
                        eventStreamEntry.SetAvatarId(requesterAvatar.GetId());
                        eventStreamEntry.SetAvatarName(requesterAvatar.GetAvatarName());
                        alliance.AddChatMessage(eventStreamEntry);

                        Parallel.ForEach((alliance.GetAllianceMembers()), op =>
                        {
                            var alliancemembers = ResourcesManager.GetPlayer(op.GetAvatarId());
                            if (alliancemembers.GetClient() != null)
                            {
                                var p = new AllianceStreamEntryMessage(alliancemembers.GetClient());
                                p.SetStreamEntry(eventStreamEntry);
                                p.Send();
                            }
                        });
                    }
                }
            }
        }

        readonly long m_vAvatarId;
        readonly string m_vMessage;
    }
}
