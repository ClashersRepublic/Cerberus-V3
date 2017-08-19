using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Magic.ClashOfClans.Files.CSV_Reader
{
    [DebuggerDisplay("Name = {" + nameof(Name) + "}")]
    public class Column
    {
        public Column(Table table, string name)
        {
            Name = name;
            Table = table ?? throw new ArgumentNullException(nameof(table));
            Table._columns.Add(this);

            _data = new List<string>();
        }

        internal readonly List<string> _data;

        public Table Table { get; }

        public string Name { get; }
    }
}
