using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Global_Chat_Entry : Message
    {
        internal string Message = string.Empty;
        internal Avatar Message_Sender = null;
        internal bool Sender = false;
        internal bool Bot = false;
        internal bool Regex = false;

        public Global_Chat_Entry(Device device) : base(device)
        {
            Identifier = 24715;
        }

        public override void Encode()
        {
            Data.AddString(Message);
            Data.AddString(Bot
                ? "Command System"
                : Sender
                    ? "You"
                    : Regex
                        ? $"[{Message_Sender.Rank}] {Message_Sender.Name}"
                        : Message_Sender.Name);

            Data.AddInt(Message_Sender.Castle_Level); // Unknown
            Data.AddInt(Bot ? 22 : Message_Sender.League);

            Data.AddLong(Message_Sender.UserId);
            Data.AddLong(Message_Sender.UserId);

            Data.AddBool(Message_Sender.ClanId > 0);
            if (Message_Sender.ClanId > 0)
            {
                var _Clan = ObjectManager.GetAlliance(Message_Sender.ClanId);
                Data.AddLong(_Clan.Clan_ID);
                Data.AddString(_Clan.Name);
                Data.AddInt(_Clan.Badge);
            }
        }
    }
}
