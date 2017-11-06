using System;

namespace CR.Servers.Files.CSV_Reader
{
    public class Row
    {
        public Row(Table table, string name)
        {
            Name = name;
            Table = table ?? throw new ArgumentNullException(nameof(table));
            Table._rows.Add(this);
        }

        public int Start;
        public int End;

        public Table Table { get; }

        public string Name { get; }
    }
}
