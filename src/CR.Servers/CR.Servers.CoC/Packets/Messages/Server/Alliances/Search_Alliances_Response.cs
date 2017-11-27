using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Search_Alliances_Response : Message
    {
        internal override short Type => 24310;

        public Search_Alliances_Response(Device device) : base(device)
        {
            
        }

        internal string TextSearch;
        internal Alliance[] Alliances;

        internal override void Encode()
        {
            this.Data.AddString(this.TextSearch);
            this.Data.AddInt(this.Alliances.Length);

            foreach (var Alliance in this.Alliances)
            {
                Alliance.Encode(this.Data);
            }
        }
    }
}
