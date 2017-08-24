using System;
using System.Collections.Generic;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Castle_Units : List<Alliance_Unit>, ICloneable
    {
        internal Castle_Units()
        {
        }

        internal Castle_Units Clone()
        {
            return MemberwiseClone() as Castle_Units;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}