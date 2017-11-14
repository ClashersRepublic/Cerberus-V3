using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Files.CSV_Reader;

namespace CR.Servers.CoC.Files.CSV_Logic.Logic
{
    internal class ExperienceLevelData : Data
    {
        public ExperienceLevelData(Row Row, DataTable DataTable) : base(Row, DataTable)
        {

        }

        public int ExpPoints { get; set; }
    }
}
