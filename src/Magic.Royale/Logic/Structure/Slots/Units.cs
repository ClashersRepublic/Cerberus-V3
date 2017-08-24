using System;
using System.Collections.Generic;
using Magic.Royale.Logic.Structure.Slots.Items;

namespace Magic.Royale.Logic.Structure.Slots
{
    internal class Units : List<Slot>, ICloneable
    {
        internal Units()
        {
            // Units.
        }
        
        internal Units Clone()
        {
            return this.MemberwiseClone() as Units;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}