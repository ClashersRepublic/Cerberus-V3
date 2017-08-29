using System.Collections.Generic;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Joinable_Alliances_List : Message
    {
        internal List<Clan> Alliances;

        public Joinable_Alliances_List(Device device) : base(device)
        {
            Identifier = 24304;
        }

        public override void Encode()
        {
            Data.AddInt(Alliances.Count);
            foreach (var alliance in Alliances)
                if (alliance != null)
                    Data.AddRange(alliance.ToBytes);
        }
    }
}
