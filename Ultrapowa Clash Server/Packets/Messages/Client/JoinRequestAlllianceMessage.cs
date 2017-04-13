using System;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class JoinRequestAllianceMessage : Message
    {
        public string Message;
        public long ID;

        public JoinRequestAllianceMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(GetData())))
            {
                ID = br.ReadInt64();
                Message = br.ReadString();
            }
        }


        public override void Process(Level level)
        {
            JoinRequestAllianceMessage requestAllianceMessage = this;
            try
            {
                ClientAvatar player = level.GetPlayerAvatar();
                Alliance all = ObjectManager.GetAlliance(ID);
                InvitationStreamEntry cm = new InvitationStreamEntry();
                cm.SetId(all.GetChatMessages().Count + 1);
                cm.SetSenderId(player.GetId());
                cm.SetHomeId(player.GetId());
                cm.SetSenderLeagueId(player.GetLeagueId());
                cm.SetSenderName(player.GetAvatarName());
                InvitationStreamEntry invitationStreamEntry = cm;
                int aRole = player.GetAllianceRole();
                invitationStreamEntry.SetSenderRole(aRole);
                invitationStreamEntry = (InvitationStreamEntry)null;
                cm.SetMessage(Message);
                cm.SetState(1);
                all.AddChatMessage(cm);

                foreach (AllianceMemberEntry op in all.GetAllianceMembers())
                {
                    Level playera = ResourcesManager.GetPlayer(op.GetAvatarId(), false);
                    if (playera.GetClient() != null)
                    {
                        var p = new AllianceStreamEntryMessage(playera.GetClient());
                        p.SetStreamEntry(cm);
                        p.Send();
                    }
                }
                List<AllianceMemberEntry>.Enumerator enumerator = new List<AllianceMemberEntry>.Enumerator();
                player = (ClientAvatar)null;
                all = (Alliance)null;
                cm = (InvitationStreamEntry)null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}