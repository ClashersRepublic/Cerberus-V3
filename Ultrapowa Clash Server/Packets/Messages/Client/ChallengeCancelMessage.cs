using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    internal class ChallengeCancelMessage : Message
    {
        public ChallengeCancelMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            // Space
        }

        public override void Process(Level level)
        {
            var a = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            var s = a.GetChatMessages().Find(c => c.GetSenderId() == level.GetPlayerAvatar().GetId() && c.GetStreamEntryType() == 12);

            if (s != null)
            {
                a.GetChatMessages().RemoveAll(t => t == s);
                foreach (AllianceMemberEntry op in a.GetAllianceMembers())
                {
                    Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (player.GetClient() != null)
                    {
                        new AllianceStreamEntryRemovedMessage(Client, s.GetId()).Send();
                    }                                                   
                }
            }
        }
    }
}
