using System.Collections.Generic;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server.Clans;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Request_Joinable_Alliances_List : Message
    {
        private const int AllianceLimit = 64;

        public Request_Joinable_Alliances_List(Device device, Reader reader) : base(device, reader)
        {
        }
       
        public override void Process()
        {
            var inMemoryAlliances = ObjectManager.GetInMemoryAlliances();

            if (inMemoryAlliances.Count == 0)
                inMemoryAlliances = ResourcesManager.LoadAllAlliance();

            var source = new List<Clan>();
            var index1 = 0;
            for (var index2 = 0; index2 < AllianceLimit && index1 < inMemoryAlliances.Count; ++index1)
                if (inMemoryAlliances[index1].Members.Count != 0 && !inMemoryAlliances[index1].IsFull)
                {
                    source.Add(inMemoryAlliances[index1]);
                    ++index2;
                }

            new Joinable_Alliances_List(Device)
            {
                Alliances = source
            }.Send();
        }
    }
}
