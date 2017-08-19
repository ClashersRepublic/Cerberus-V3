using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Trap : Construction_Item
    {
        public Trap(Data data, Level l) : base(data, l)
        {
            /*AddComponent(new Trigger_Component());
            if (GetTrapData.HasAltMode || GetTrapData.DirectionCount > 0)
            {
                AddComponent(new Combat_Component(this));
            }*/
        }

        internal override bool Builder => false;
        internal override int ClassId => 4;

        public Traps GetTrapData => (Traps)Data;
    }
}