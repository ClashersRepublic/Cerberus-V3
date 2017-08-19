using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Deco : Game_Object
    {
        public Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override bool Builder => false;
        internal override int ClassId => 6;

        public Decos GetDecoData() => (Decos)Data;
    }
}