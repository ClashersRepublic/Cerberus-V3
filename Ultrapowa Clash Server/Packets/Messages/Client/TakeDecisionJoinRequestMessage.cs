﻿using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing.Commands.Server;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14321
    internal class TakeDecisionJoinRequestMessage : Message
    {
        public TakeDecisionJoinRequestMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public long MessageID { get; set; }

        public int Choice { get; set; }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(GetData())))
            {
                MessageID = br.ReadInt64();
                Choice = br.ReadByte();
            }
        }

        public override void Process(Level level)
        {

            Alliance a = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            StreamEntry message = a.GetChatMessages().Find(c => c.GetId() == MessageID);
            Level requester = ResourcesManager.GetPlayer(message.GetSenderId());
            if (Choice == 1)
            {
                if (!a.IsAllianceFull())
                {
                    requester.GetPlayerAvatar().SetAllianceId(a.GetAllianceId());

                    AllianceMemberEntry member = new AllianceMemberEntry(requester.GetPlayerAvatar().GetId());
                    member.SetRole(1);
                    a.AddAllianceMember(member);

                    StreamEntry e = a.GetChatMessages().Find(c => c.GetId() == MessageID);
                    e.SetJudgeName(level.GetPlayerAvatar().GetAvatarName());
                    e.SetState(2);

                    AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                    eventStreamEntry.SetId(a.GetChatMessages().Count + 1);
                    eventStreamEntry.SetSender(requester.GetPlayerAvatar());
                    eventStreamEntry.SetAvatarName(level.GetPlayerAvatar().GetAvatarName());
                    eventStreamEntry.SetAvatarId(level.GetPlayerAvatar().GetId());
                    eventStreamEntry.SetEventType(2);

                    a.AddChatMessage(eventStreamEntry);

                    // New function for send a message
                    foreach (AllianceMemberEntry op in a.GetAllianceMembers())
                    {
                        Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                        if (player.GetClient() != null)
                        {
                            AllianceStreamEntryMessage c = new AllianceStreamEntryMessage(player.GetClient());
                            AllianceStreamEntryMessage p = new AllianceStreamEntryMessage(player.GetClient());
                            p.SetStreamEntry(eventStreamEntry);
                            c.SetStreamEntry(e);
                            p.Send();
                            c.Send();
                        }
                    }
                    if (ResourcesManager.IsPlayerOnline(requester))
                    {
                        JoinedAllianceCommand joinAllianceCommand = new JoinedAllianceCommand();
                        joinAllianceCommand.SetAlliance(a);

                        AvailableServerCommandMessage availableServerCommandMessage = new AvailableServerCommandMessage(requester.GetClient());
                        availableServerCommandMessage.SetCommandId(1);
                        availableServerCommandMessage.SetCommand(joinAllianceCommand);

                        AllianceRoleUpdateCommand d = new AllianceRoleUpdateCommand();
                        d.SetAlliance(a);
                        d.SetRole(4);
                        d.Tick(level);

                        AvailableServerCommandMessage c = new AvailableServerCommandMessage(Client);
                        c.SetCommandId(8);
                        c.SetCommand(d);

                        new AnswerJoinRequestAllianceMessage(Client).Send();
                        availableServerCommandMessage.Send();
                        c.Send();
                        new AllianceStreamMessage(requester.GetClient(), a).Send();
                    }
                }
            }
            else
            {
                StreamEntry e = a.GetChatMessages().Find(c => c.GetId() == MessageID);
                e.SetJudgeName(level.GetPlayerAvatar().GetAvatarName());
                e.SetState(3);

                // New function for send a message
                foreach (AllianceMemberEntry op in a.GetAllianceMembers())
                {
                    Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (player.GetClient() != null)
                    {
                        AllianceStreamEntryMessage c = new AllianceStreamEntryMessage(player.GetClient());
                        c.SetStreamEntry(e);
                        PacketManager.Send(c);
                    }
                }
            }
        }

    }
}