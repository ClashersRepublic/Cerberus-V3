using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    // Packet 14715
    internal class SendGlobalChatLineMessage : Message
    {
        public SendGlobalChatLineMessage(PacketProcessing.Client client, PacketReader br) : base(client, br)
        {
        }

        public string Message { get; set; }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(GetData())))
            {
                Message = br.ReadString();
            }
        }

        public override void Process(Level level)
        {
            if (Message.Length > 0)
            {
                if (Message[0] == '/')
                {
                    var obj = GameOpCommandFactory.Parse(Message);
                    if (obj != null)
                    {
                        var player = "";
                        if (level != null)
                            player += " (" + level.GetPlayerAvatar().GetId() + ", " +
                                      level.GetPlayerAvatar().GetAvatarName() + ")";
                        ((GameOpCommand) obj).Execute(level);
                    }
                }
                else
                {
                    if (File.Exists(@"filter.ucs"))
                    {
                        var senderId = level.GetPlayerAvatar().GetId();
                        var senderName = level.GetPlayerAvatar().GetAvatarName();

                        var badwords = new List<string>();
                        var r = new StreamReader(@"filter.ucs");
                        var line = "";
                        while ((line = r.ReadLine()) != null)
                        {
                            badwords.Add(line);
                        }
                        var badword = badwords.Any(s => Message.Contains(s));

                        if (badword)
                        {
                            var p = new GlobalChatLineMessage(level.GetClient());
                            p.SetPlayerId(0);
                            p.SetPlayerName("Chat Filter System");
                            p.SetLeagueId(22);
                            p.SetChatMessage("DETECTED BAD WORD! PLEASE AVOID USING BAD WORDS!");
                            p.Send();
                            return;
                        }

                        Parallel.ForEach((ResourcesManager.GetOnlinePlayers()), (onlinePlayer, l) =>
                        {
                            var p = new GlobalChatLineMessage(onlinePlayer.GetClient());
                            var p2 = onlinePlayer.GetPlayerAvatar().GetPremium();
                            if (onlinePlayer.GetAccountPrivileges() > 0)
                            {
                                p.SetPlayerName(senderName + " #" + senderId);
                            }
                            else
                            {
                                p.SetPlayerName(senderName);
                            }
                            p.SetChatMessage(Message);
                            p.SetPlayerId(senderId);
                            p.SetLeagueId(level.GetPlayerAvatar().GetLeagueId());
                            p.SetAlliance(ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId()));
                            p.Send();
                            Logger.Write("Chat Message: '" + Message + "' from '" + senderName + "':'" + senderId + "'");
                        });
                    }
                }
            }
        }
    }
}
