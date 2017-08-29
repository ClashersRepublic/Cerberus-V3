

using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;
using Magic.ClashOfClans.Network.Messages.Server.Errors;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Request_Join_Alliance : Message
    {
        internal long AllianceID;
        internal string Message;

        public Request_Join_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            AllianceID = Reader.ReadInt64();
            Message = Reader.ReadString();
        }

        public override void Process()
        {
            if (AllianceID > 0)
            {
                var clan = ObjectManager.GetAlliance(AllianceID);
                if (clan != null)
                    clan.Chats.Add(
                        new Entry
                        {
                            Stream_Type = Alliance_Stream.INVITATION,
                            Sender_ID = Device.Player.Avatar.UserId,
                            Sender_Name = Device.Player.Avatar.Name,
                            Sender_Level = Device.Player.Avatar.Level,
                            Sender_League = Device.Player.Avatar.League,
                            Sender_Role = Role.Member,
                            Message = Message
                        });
                else
                    new Out_Of_Sync(Device).Send();
            }
        }
    }
}
