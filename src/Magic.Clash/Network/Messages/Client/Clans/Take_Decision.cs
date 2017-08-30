using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.ClashOfClans.Network.Messages.Server.Clans;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Take_Decision : Message
    {
        internal int Stream_High_ID;
        internal int Stream_Low_ID;
        internal byte Decision;

        public Take_Decision(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            Stream_High_ID = Reader.ReadInt32();
            Stream_Low_ID = Reader.ReadInt32();
            Decision = Reader.ReadByte();
        }

        public override void Process()
        {
            var Alliance = ObjectManager.GetAlliance(Device.Player.Avatar.ClanId);
            var Stream = Alliance.Chats.Get(Stream_Low_ID);
            if (Stream != null)
            {
                var Player = ResourcesManager.GetPlayer(Stream.Sender_ID);
                if (Player.Avatar.ClanId == 0)
                {
                    if (Decision == 1)
                        if (Alliance.Members.Count < 50)
                        {
                            Player.Avatar.ClanId = Alliance.Clan_ID;
                            Player.Avatar.Alliance_Name = Alliance.Name;
                            Player.Avatar.Alliance_Level = Alliance.Level;
                            Player.Avatar.Alliance_Role = (int) Role.Member;
                            Player.Avatar.Badge_ID = Alliance.Badge;

                            foreach (var Old_Entry in Player.Avatar.Inbox.Slots.FindAll(M => M.Stream_Type == Logic.Enums.Avatar_Stream.INVITATION)
                            )
                            {
                                Player.Avatar.Inbox.Remove(Old_Entry);
                            }

                            Alliance.Members.Add(Player.Avatar);

                            if (Player.Device != null)
                            {
                                new Server_Commands(Player.Device)
                                {
                                    Command = new Joined_Alliance(Player.Device) {Clan = Alliance}
                                }.Send();

                                new Alliance_All_Stream_Entry(Player.Device).Send();
                            }

                            Alliance.Chats.Add(new Entry
                            {
                                Stream_Type = Alliance_Stream.EVENT,
                                Sender_ID = Player.Avatar.UserId,
                                Sender_Name = Player.Avatar.Name,
                                Sender_Level = Player.Avatar.Level,
                                Sender_League = Player.Avatar.League,
                                Sender_Role = Role.Member,
                                Event_ID = Events.ACCEPT_MEMBER,
                                Event_Player_ID = Device.Player.Avatar.UserId,
                                Event_Player_Name = Device.Player.Avatar.Name
                            });
                        }

                    Stream.Judge_Name = Device.Player.Avatar.Name;
                    Stream.Stream_State = Decision == 1 ? InviteState.ACCEPTED : InviteState.REFUSED;

                    foreach (var Member in Alliance.Members.Values)
                        if (Member.Connected)
                            new Alliance_Stream_Entry(Member.Player.Device, Stream).Send();
                    
                    DatabaseManager.Save(Alliance);
                    DatabaseManager.Save(Player);
                }
                else
                {
                    Alliance.Chats.Remove(Stream);
                }
            }
        }
    }
}
