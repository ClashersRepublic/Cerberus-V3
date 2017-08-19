using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Village_Object : Construction_Item
    {
        public Village_Object(Data data, Level level) : base(data, level)
        {
        }

        internal override bool Builder => false;
        internal override int ClassId => 8;

        public Village_Objects GetVillageObjectsData => (Village_Objects)Data;
    }
}
