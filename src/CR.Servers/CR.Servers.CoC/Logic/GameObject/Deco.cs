namespace CR.Servers.CoC.Logic
{
    using CR.Servers.CoC.Files.CSV_Helpers;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;

    internal class Deco : GameObject
    {
        public Deco(Data Data, Level Level) : base(Data, Level)
        {
        }

        internal DecoData DecoData => (DecoData) this.Data;

        internal override int HeightInTiles => this.DecoData.Height;

        internal override int WidthInTiles => this.DecoData.Width;

        internal override int Type => 6;

        internal override int VillageType => this.DecoData.VillageType;
    }
}