﻿namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Duel.Entry;

    internal class Village2AttackEntryAddedMessage : Message
    {
        internal Village2AttackEntry Entry;

        public Village2AttackEntryAddedMessage(Device Device, Village2AttackEntry entry) : base(Device)
        {
            this.Entry = entry;
        }

        internal override short Type
        {
            get
            {
                return 24372;
            }
        }

        internal override void Encode()
        {
        }
    }
}