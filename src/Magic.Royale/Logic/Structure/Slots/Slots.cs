using System;
using System.Collections.Generic;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Slots : List<Slot>, ICloneable
    {
        internal Slots Clone()
        {
            return this.MemberwiseClone() as Slots;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}