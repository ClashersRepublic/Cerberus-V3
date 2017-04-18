using System;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.Logic.StreamEntries;
using Magic.PacketProcessing.Messages.Server;
using System.Threading.Tasks;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class ChatToAllianceStreamMessage : Message
    {
        string m_vChatMessage;

        public ChatToAllianceStreamMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                m_vChatMessage = br.ReadScString();
            }
        }

        public override void Process(Level level)
        {
            if (m_vChatMessage.Length > 0)
            {
                if (m_vChatMessage[0] == '/')
                {
                    var obj = GameOpCommandFactory.Parse(m_vChatMessage);
                    if (obj != null)
                    {
                        var player = "";
                        if (level != null)
                            player += " (" + level.Avatar.Id+ ", " +
                                      level.Avatar.GetAvatarName() + ")";
                        ((GameOpCommand)obj).Execute(level);
                    }
                }
                else
                {
                    var avatar = level.Avatar;
                    var allianceId = avatar.GetAllianceId();
                    if (allianceId > 0)
                    {
                        var cm = new ChatStreamEntry();
                        cm.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        cm.SetSender(avatar);
                        cm.SetMessage(m_vChatMessage);

                        var alliance = ObjectManager.GetAlliance(allianceId);
                        if (alliance != null)
                        {
                            alliance.AddChatMessage(cm);

                            Parallel.ForEach ((alliance.AllianceMembers), op =>
                            {
                                Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                                if (player.Client!= null)
                                {
                                    var p = new AllianceStreamEntryMessage(player.Client);
                                    p.SetStreamEntry(cm);
                                    p.Send();
                                }
                            });
                        }
                    }
                }
            }
        }
    }
}