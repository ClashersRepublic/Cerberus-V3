using System.Collections.Generic;
using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;
using Microsoft.VisualBasic.CompilerServices;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Search_Alliances : Message
    {
        internal override short Type => 14324;

        public Search_Alliances(Device device, Reader reader) : base(device, reader)
        {
        }

        internal string TextSearch;
        internal int WarFrequency;
        internal int ClanLocation;
        internal int MinimumMembers;
        internal int MaximumMembers;
        internal int RequiredScore;
        internal int RequiredDuelScore;
        internal bool OnlyCanJoin;
        internal int Unknown1;
        internal int ExpLevels;

        internal override void Decode()
        {
            this.TextSearch = this.Reader.ReadString();
            this.WarFrequency = this.Reader.ReadInt32();
            this.ClanLocation = this.Reader.ReadInt32();
            this.MinimumMembers = this.Reader.ReadInt32();
            this.MaximumMembers = this.Reader.ReadInt32();
            this.RequiredScore = this.Reader.ReadInt32();
            this.RequiredDuelScore = this.Reader.ReadInt32();
            this.OnlyCanJoin = this.Reader.ReadBoolean();
            this.Unknown1 = this.Reader.ReadInt32();
            this.ExpLevels = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            ShowValues();
            new Search_Alliances_Response(this.Device)
            { 
                TextSearch = this.TextSearch,
                Alliances = Resources.Clans.Values.Where(T =>
                    LikeOperator.LikeString(T.Header.Name, this.TextSearch, Microsoft.VisualBasic.CompareMethod.Text) &&
                    T.Header.NumberOfMembers >= this.MinimumMembers &&
                    T.Header.NumberOfMembers <= this.MaximumMembers &&
                    T.Header.ExpLevel >= this.ExpLevels &&
                    T.Header.Type <= (this.OnlyCanJoin ? Hiring.INVITE : Hiring.CLOSED) && //TODO: Check required trophy stuff
                    T.Header.Origin == (this.ClanLocation == 0 ? T.Header.Origin : this.ClanLocation) &&
                    T.Header.WarFrequency == (this.WarFrequency == 0 ? T.Header.WarFrequency : this.WarFrequency) &&
                    T.Header.Score >= this.RequiredScore &&
                    T.Header.DuelScore >= this.RequiredDuelScore)
                .Take(64).OrderByDescending(T => T.Header.Score).ToArray()          
            }.Send();
        }
    }
}
