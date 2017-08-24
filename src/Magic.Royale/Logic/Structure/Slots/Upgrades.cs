using System;
using System.Collections.Generic;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Upgrades : List<Slot>, ICloneable
    {
        internal Upgrades()
        {
            // Upgrades.
        }

        internal Upgrades Clone()
        {
            return this.MemberwiseClone() as Upgrades;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}