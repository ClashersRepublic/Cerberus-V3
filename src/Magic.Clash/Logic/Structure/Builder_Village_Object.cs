using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Builder_Village_Object : Construction_Item
    {
        public Builder_Village_Object(Data data, Level level) : base(data, level)
        {
        }


        internal override bool Builder => true;
        internal override int ClassId => 15;

        public Village_Objects GetVillageObjectsData => (Village_Objects)Data;
    }
}
