using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Files.CSV_Reader;

namespace CR.Servers.CoC.Files.CSV_Logic.Logic
{
    internal class VariableData : Data
    {
        public VariableData(Row Row, DataTable DataTable) : base(Row, DataTable)
        {
        }

        public override string Name { get; set; }
        public int Value { get; set; }
    }
}
