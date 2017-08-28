using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Add_Alliance_Message : Message
    {
        internal string Message = string.Empty;

        public Add_Alliance_Message(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            Message = Reader.ReadString();
        }

        public override void Process()
        {
            var clan = ObjectManager.GetAlliance(Device.Player.Avatar.ClanId);
            clan.Chats.Add(
                new Entry
                {
                    Stream_Type = Alliance_Stream.CHAT,
                    Sender_ID = Device.Player.Avatar.UserId,
                    Sender_Name = Device.Player.Avatar.Name,
                    Sender_Level = Device.Player.Avatar.Level,
                    Sender_League = Device.Player.Avatar.League,
                    Sender_Role = clan.Members[Device.Player.Avatar.UserId].Role,
                    Message = Message
                });
        }
    }
}