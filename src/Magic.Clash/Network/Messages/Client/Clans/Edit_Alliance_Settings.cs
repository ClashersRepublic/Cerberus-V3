using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Edit_Alliance_Settings : Message
    {
        internal string Message = string.Empty;

        internal Clan Clan;

        public Edit_Alliance_Settings(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            Clan = ObjectManager.GetAlliance(Device.Player.Avatar.ClanId);

            if (Clan != null)
            {
                Clan.Description = Reader.ReadString();
                Reader.ReadInt32();
                Clan.Badge = Reader.ReadInt32();
                Clan.Type = (Hiring) Reader.ReadInt32();
                Clan.Required_Trophies = Reader.ReadInt32();
                Clan.War_Frequency = Reader.ReadInt32();
                Clan.Origin = Reader.ReadInt32();

                switch (Reader.ReadByte())
                {
                    case 0:
                        Clan.War_History = false;
                        Clan.War_Amical = false;
                        break;
                    case 1:
                        Clan.War_History = true;
                        Clan.War_Amical = false;
                        break;
                    case 2:
                        Clan.War_History = false;
                        Clan.War_Amical = true;
                        break;
                    case 3:
                        Clan.War_History = true;
                        Clan.War_Amical = true;
                        break;
                }
            }
        }

        public override void Process()
        {
            if (Clan != null)
            {
                Clan.Chats.Add(
                    new Entry
                    {
                        Stream_Type = Alliance_Stream.EVENT,
                        Sender_ID = Device.Player.Avatar.UserId,
                        Sender_Name = Device.Player.Avatar.Name,
                        Sender_Level = Device.Player.Avatar.Level,
                        Sender_League = Device.Player.Avatar.League,
                        Sender_Role = Clan.Members[Device.Player.Avatar.UserId].Role,
                        Event_ID = Events.UPDATE_SETTINGS,
                        Event_Player_ID = Device.Player.Avatar.UserId,
                        Event_Player_Name = Device.Player.Avatar.Name
                    });

                new Server_Commands(Device) {Command = new Changed_Alliance_Setting(Device) {Clan = Clan}.Handle()}
                    .Send();
            }
            else
            {
                new Out_Of_Sync(Device).Send();
            }
        }
    }
}