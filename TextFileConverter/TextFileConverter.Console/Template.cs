using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileConverter.Console
{
    public enum Pad
    {
        Left,
        Right,
        None
    }

    public class Column
    {
        public string HeaderText { get; set; }
        public bool IsFixedWidth { get; set; }
        public int MaxWidth { get; set; }
        public Pad Pad { get; set; }
    }

    public class Template
    {       
        public bool IsFirstLineColumnHeader { get; set; }
        public char ColumnSeperator { get; set; }        
        public List<Column> Columns { get; set; }
        public int ColumnCount => Columns.Count;       
    }

    public class Input : Template
    {

    }

    public class Output : Template
    {
        public bool IsFixedWidth { get; set; }
        public int LineWidth { get; set; }
        public char RowHeader { get; set; }
        public char RowTerminator { get; set; }
        public int ColumnPadding { get; set; }
        public string TruncatedMarker { get; set; }
    }

}
