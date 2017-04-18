using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.StreamEntries;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.Packets.Messages.Client
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
            var a = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            var s = a.ChatMessages.Find(c => c.GetSenderId() == level.Avatar.Id&& c.GetStreamEntryType() == 12);

            if (s != null)
            {
                a.ChatMessages.RemoveAll(t => t == s);
                foreach (AllianceMemberEntry op in a.AllianceMembers)
                {
                    Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (player.Client!= null)
                    {
                        new AllianceStreamEntryRemovedMessage(Client, s.GetId()).Send();
                    }                                                   
                }
            }
        }
    }
}
