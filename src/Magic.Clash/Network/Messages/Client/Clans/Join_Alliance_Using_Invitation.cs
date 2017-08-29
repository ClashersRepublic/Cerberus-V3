using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.ClashOfClans.Network.Messages.Server.Clans;
using Avatar_Stream = Magic.ClashOfClans.Logic.Enums.Avatar_Stream;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Join_Alliance_Using_Invitation : Message
    {
        internal long InviteID;
        internal long InviterID;
        public Join_Alliance_Using_Invitation(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            InviteID = Reader.ReadInt64();
            InviterID = Reader.ReadInt64();
        }

        public override void Process()
        {
            var avatar = Device.Player.Avatar;
            if (avatar.ClanId == 0)
            {
                int Index = avatar.Inbox.Slots.FindIndex(
                    M => M.Sender_ID == InviterID && M.Stream_Type == Avatar_Stream.INVITATION);
                if (Index > -1)
                {
                    var ClanID = avatar.Inbox.Slots[Index].Alliance_ID;
                    var Alliance = ObjectManager.GetAlliance(ClanID);

                    if (Alliance != null)
                    {
                        foreach (var Old_Entry in avatar.Inbox.Slots.FindAll(
                            M => M.Stream_Type == Avatar_Stream.INVITATION)
                        )
                        {
                            avatar.Inbox.Remove(Old_Entry);
                        }

                        Alliance.Members.Add(avatar);
                        avatar.ClanId = Alliance.Clan_ID;
                        avatar.Alliance_Level = Alliance.Level;
                        avatar.Alliance_Name = Alliance.Name;
                        avatar.Alliance_Role = (int) Role.Member;
                        avatar.Badge_ID = Alliance.Badge;


                        new Server_Commands(Device)
                        {
                            Command = new Joined_Alliance(Device) {Clan = Alliance}.Handle()
                        }.Send();

                        new Alliance_Full_Entry(Device).Send();
                        new Alliance_All_Stream_Entry(Device).Send();

                        Alliance.Chats.Add(new Entry
                        {
                            Stream_Type = Alliance_Stream.EVENT,
                            Sender_ID = avatar.UserId,
                            Sender_Name = avatar.Name,
                            Sender_Level = avatar.Level,
                            Sender_League = avatar.League,
                            Sender_Role = Role.Member,
                            Event_ID = Events.JOIN_ALLIANCE,
                            Event_Player_ID = avatar.UserId,
                            Event_Player_Name = avatar.Name
                        });

                        DatabaseManager.Save(Alliance);
                        DatabaseManager.Save(Device.Player);
                    }
                }
            }
        }
    }
}
