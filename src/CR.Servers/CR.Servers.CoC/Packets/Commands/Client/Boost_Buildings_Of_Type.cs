﻿using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Boost_Buildings_Of_Type : Command
    {
        internal override int Type => 584;

        public Boost_Buildings_Of_Type(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int BuildingType;

        internal override void Decode()
        {
            this.BuildingType = this.Reader.ReadInt32();
            base.Decode();
        }
    }
}