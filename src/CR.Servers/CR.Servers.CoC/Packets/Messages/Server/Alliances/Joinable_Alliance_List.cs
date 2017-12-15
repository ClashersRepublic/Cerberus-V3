﻿using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Alliances
{
    internal class Joinable_Alliance_List : Message
    {
        internal override short Type => 24304;

        public Joinable_Alliance_List(Device Device) : base(Device)
        {
        }

        internal Alliance[] Alliances;

        internal override void Encode()
        {
            this.Data.AddInt(this.Alliances.Length);
            foreach (var Alliance in this.Alliances)
            {
                Alliance.Header.Encode(this.Data);
            }

            this.Data.AddInt(0); //Another list but no one know what is it for
        }

    }
}
