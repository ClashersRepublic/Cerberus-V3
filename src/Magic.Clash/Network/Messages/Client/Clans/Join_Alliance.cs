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
    internal class Join_Alliance : Message
    {
        internal long ClanID;

        public Join_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            ClanID = Reader.ReadInt64();
        }

        public override void Process()
        {
            var Alliance = ObjectManager.GetAlliance(ClanID);

            if (Alliance != null)
            {
                var avatar = Device.Player;
                foreach (var Old_Entry in avatar.Avatar.Inbox.Slots.FindAll(M => M.Stream_Type == Avatar_Stream.INVITATION))
                {
                    avatar.Avatar.Inbox.Remove(Old_Entry);
                }

                Alliance.Members.Add(avatar.Avatar);
                avatar.Avatar.ClanId = Alliance.Clan_ID;
                avatar.Avatar.Alliance_Level = Alliance.Level;
                avatar.Avatar.Alliance_Name = Alliance.Name;
                avatar.Avatar.Alliance_Role = (int) Role.Member;
                avatar.Avatar.Badge_ID = Alliance.Badge;


                new Server_Commands(Device)
                {
                    Command = new Joined_Alliance(Device) {Clan = Alliance}.Handle()
                }.Send();

                new Alliance_Full_Entry(Device).Send();
                new Alliance_All_Stream_Entry(Device).Send();

                Alliance.Chats.Add(new Entry
                {
                    Stream_Type = Alliance_Stream.EVENT,
                    Sender_ID = avatar.Avatar.UserId,
                    Sender_Name = avatar.Avatar.Name,
                    Sender_Level = avatar.Avatar.Level,
                    Sender_League = avatar.Avatar.League,
                    Sender_Role = Role.Member,
                    Event_ID = Events.JOIN_ALLIANCE,
                    Event_Player_ID = avatar.Avatar.UserId,
                    Event_Player_Name = avatar.Avatar.Name
                });

                DatabaseManager.Save(Alliance);
                DatabaseManager.Save(avatar);
            }
        }
    }
}
