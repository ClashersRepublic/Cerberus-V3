using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing;
using UCS.PacketProcessing.Messages.Server;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    internal class ChallangeAttackMessage : Message
    {
        public ChallangeAttackMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public long ID { get; set; }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(GetData())))
            {
                ID = br.ReadInt64WithEndian();
            }
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
