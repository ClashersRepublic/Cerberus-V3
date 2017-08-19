using Magic.ClashOfClans.Files.CSV_Helpers;
using Magic.ClashOfClans.Files.CSV_Logic;

namespace Magic.ClashOfClans.Logic.Structure
{
    internal class Builder_Deco : Game_Object
    {
        public Builder_Deco(Data data, Level l) : base(data, l)
        {
        }

        internal override bool Builder => true;
        internal override int ClassId => 13;

        public Decos GetDecoData => (Decos)Data;

    }
}