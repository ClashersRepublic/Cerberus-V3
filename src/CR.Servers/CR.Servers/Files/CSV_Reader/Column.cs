using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CR.Servers.Files.CSV_Reader
{
    [DebuggerDisplay("Name = {" + nameof(Name) + "}")]
    public class Column
    {
        public Column(Table table, string name)
        {
            Name = name;
            Table = table ?? throw new ArgumentNullException(nameof(table));
            Table._columns.Add(this);

            Data = new List<string>();
        }

        public readonly List<string> Data;

        public Table Table { get; }

        public string Name { get; }
    }
}
