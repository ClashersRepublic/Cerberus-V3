using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Trap : Construction_Item
    {
        public Trap(Data data, Level l) : base(data, l)
        {
            if (GetTrapData.HasAltMode || GetTrapData.DirectionCount > 0)
            {
                AddComponent(new Combat_Component(this));
            }
        }

        internal override bool Builder => false;
        internal override int ClassId => 4;
        internal override int OppositeClassId => 11;
        public Traps GetTrapData => (Traps)Data;
    }
}