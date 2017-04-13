using UCS.Files.CSV;

namespace UCS.Files.Logic
{
    internal class EffectsData : Data
    {
        public EffectsData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
    }
}
