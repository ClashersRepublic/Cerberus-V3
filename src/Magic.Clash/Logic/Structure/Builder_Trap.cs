using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Components;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Builder_Trap : Construction_Item
    {
        public Builder_Trap(Data data, Level l) : base(data, l)
        {
            //AddComponent(new Trigger_Component());
            if (GetTrapData.HasAltMode || GetTrapData.DirectionCount > 0)
            {
                AddComponent(new Combat_Component(this));
            }
        }

        internal override bool Builder => true;
        internal override int ClassId => 11;

        public Traps GetTrapData => (Traps)Data;
    }
}