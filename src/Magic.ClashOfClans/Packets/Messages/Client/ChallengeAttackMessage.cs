using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing;
using Magic.PacketProcessing.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeAttackMessage : Message
    {
        public ChallengeAttackMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public long ID;

        public override void Decode()
        {
            ID = Reader.ReadInt64WithEndian();
        }

        public override void Process(Level level)
        {
            var a = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            var defender = ResourcesManager.GetPlayer(a.GetChatMessages().Find(c => c.GetId() == ID).GetSenderId());
            if (defender != null)
            {
                defender.Tick();
                new ChallangeAttackDataMessage(Client, defender).Send();
            }
            else
            {
                new OwnHomeDataMessage(Client, level);
            }

            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            var s = alliance.GetChatMessages().Find(c => c.GetStreamEntryType() == 12);
            if (s != null)
            {
                alliance.GetChatMessages().RemoveAll(t => t == s);

                foreach (AllianceMemberEntry op in alliance.GetAllianceMembers())
                {
                    Level playera = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (playera.GetClient() != null)
                    {
                        var p = new AllianceStreamEntryMessage(playera.GetClient());
                        p.SetStreamEntry(s);
                        p.Send();
                    }
                }
            }
        }
    }
}
