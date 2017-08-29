using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Search_Response : Message
    {
        internal string TextSearch;
        internal List<Clan> Alliances;

        public Alliance_Search_Response(Device device) : base(device)
        {
            Identifier = 24310;
        }

        public override void Encode()
        {
            Data.AddString(TextSearch);
            Data.AddInt(Alliances.Count);
            foreach (var alliance in Alliances)
                if (alliance != null)
                    Data.AddRange(alliance.ToBytes);
        }
    }
}
