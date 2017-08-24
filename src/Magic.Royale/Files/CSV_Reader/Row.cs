using System;

namespace Magic.Royale.Files.CSV_Reader
{
    public class Row
    {
        public Row(Table table, string name)
        {
            Name = name;
            Table = table ?? throw new ArgumentNullException(nameof(table));
            Table._rows.Add(this);
        }

        internal int _start;
        internal int _end;

        public Table Table { get; }

        public string Name { get; }
    }
}
