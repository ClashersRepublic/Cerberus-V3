using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure.Slots.Items;

namespace Magic.ClashOfClans.Network.Commands.Client.Clan
{
    internal class Send_Mail : Command
    {
        internal string Message;
        internal int Tick;


        public Send_Mail(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            Message = Reader.ReadString();
            Reader.ReadInt32(); //Tick
        }

        public override void Process()
        {
            var AllianceID = Device.Player.Avatar.ClanId;

            if (AllianceID > 0)
            {
                var Clan = ObjectManager.GetAlliance(AllianceID);
                if (Clan != null)
                {
                    var Mail = new Mail
                    {
                        Stream_Type = Avatar_Stream.CLAN_MAIL,
                        Sender_ID = Device.Player.Avatar.UserId,
                        Sender_Name = Device.Player.Avatar.Name,
                        Sender_Level = Device.Player.Avatar.Level,
                        Sender_League = Device.Player.Avatar.League,
                        Alliance_ID = AllianceID,
                        Message = Message
                    };

                    foreach (var Member in Clan.Members.Values)
                        Member?.Player?.Avatar.Inbox.Add(Mail);
                }
            }
        }
    }
}
