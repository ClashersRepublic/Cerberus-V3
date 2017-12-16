 using System.Linq;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Leave_Alliance : Message
    {
        internal bool done;

        public Leave_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Process()
        {
            var User = Device.Player.Avatar;
            var Alliance = ObjectManager.GetAlliance(User.ClanId);

            if (Alliance != null)
            {
                User.ClanId = 0;
                User.Alliance_Level = -1;
                User.Alliance_Name = string.Empty;
                User.Alliance_Role = -1;
                User.Badge_ID = -1;

                if (Alliance.Members[User.UserId].Role == Role.Leader && Alliance.Members.Count > 1)
                {
                    foreach (var player in Alliance.Members.Values.Where(player => player.Role >= Role.Elder))
                    {
                        player.Role = Role.Leader;

                        if (player.Player.Device != null)
                            new Server_Commands(player.Player.Device)
                            {
                                Command = new Role_Update(player.Player.Device)
                                {
                                    Clan = Alliance,
                                    Role = (int) Role.Leader
                                }.Handle()
                            }.Send();
                        done = true;
                        break;
                    }

                    if (!done)
                    {
                        var UserID = Alliance.Members.Keys.SkipWhile(P => P == User.UserId).ToList();

                        while (UserID.Count > 1)
                        {
                            var index = ObjectManager.Random.Next(0, UserID.Count);
                            UserID.RemoveAt(index);
                        }
                        var lucky = Alliance.Members[UserID[0]];
                        lucky.Role = Role.Leader;
                        if (lucky.Player.Device != null)
                            new Server_Commands(lucky.Player.Device)
                            {
                                Command = new Role_Update(lucky.Player.Device)
                                {
                                    Clan = Alliance,
                                    Role = (int) Role.Leader
                                }.Handle()
                            }.Send();
                    }
                }
               

                Alliance.Members.Remove(Device.Player.Avatar);
                new Server_Commands(Device)
                {
                    Command = new Leaved_Alliance(Device) {AllianceID = Alliance.Clan_ID, Reason = 1}.Handle()
                }.Send();

                if (Alliance.Members.Count == 0)
                {
                    //DatabaseManager.RemoveAlliance(Alliance);
                }
                else
                {
                    Alliance.Chats.Add(new Entry
                    {
                        Stream_Type = Alliance_Stream.EVENT,
                        Sender_ID = Device.Player.Avatar.UserId,
                        Sender_Name = Device.Player.Avatar.Name,
                        Sender_Level = Device.Player.Avatar.Level,
                        Sender_League = Device.Player.Avatar.League,
                        Sender_Role = Role.Member,
                        Event_ID = Events.LEAVE_ALLIANCE,
                        Event_Player_ID = Device.Player.Avatar.UserId,
                        Event_Player_Name = Device.Player.Avatar.Name
                    });
                }
                DatabaseManager.Save(Alliance);
                DatabaseManager.Save(Device.Player);
            }
        }
    }
}
