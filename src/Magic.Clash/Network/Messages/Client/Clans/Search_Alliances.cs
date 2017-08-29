using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Network.Messages.Server.Clans;

namespace Magic.ClashOfClans.Network.Messages.Client.Clans
{
    internal class Search_Alliances : Message
    {
        internal string TextSearch;
        internal int WarFrequency;
        internal int ClanLocation;
        internal int MinimumMembers;
        internal int MaximumMembers;
        internal int TrophyLimit;
        internal bool OnlyCanJoin;
        internal int Unknown1;
        internal int ExpLevels;

        public Search_Alliances(Device device, Reader reader) : base(device, reader)
        {
        }

        public override void Decode()
        {
            TextSearch = Reader.ReadString();
            WarFrequency = Reader.ReadInt32();
            ClanLocation = Reader.ReadInt32();
            MinimumMembers = Reader.ReadInt32();
            MaximumMembers = Reader.ReadInt32();
            TrophyLimit = Reader.ReadInt32();
            OnlyCanJoin = Reader.ReadBoolean();
            Unknown1 = Reader.ReadInt32();        
            ExpLevels = Reader.ReadInt32();
        }

        public override void Process()
        {
            var inMemoryAlliances = ObjectManager.GetInMemoryAlliances();

            if (inMemoryAlliances.Count == 0)
                inMemoryAlliances = ResourcesManager.LoadAllAlliance();

            var joinableAlliances = new List<Clan>();
            for (int i = 0; i < inMemoryAlliances.Count; i++)
            {
                if (joinableAlliances.Count >= 64)
                    break;

                var alliance = inMemoryAlliances[i];
                if (TextSearch == null)
                {
                    joinableAlliances.Add(alliance);
                }
                else
                {
                    if (alliance.Name.Contains(TextSearch, StringComparison.OrdinalIgnoreCase))
                    {
                        joinableAlliances.Add(alliance);
                    }
                }
            }
            new Alliance_Search_Response(Device)
            {
                TextSearch = TextSearch,
                Alliances = joinableAlliances
            }.Send();


        }
    }
}
