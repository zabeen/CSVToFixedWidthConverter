using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TextFileConverter.Library
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
        public string CultureName { get; set; }
    }

    public class Template
    {       
        public bool IsFirstLineHeader { get; set; }
        public char ColumnSeperator { get; set; }
    }

    public class Input : Template
    {
        public class InColumn : Column
        {
            
        }
        public List<InColumn> Columns { get; set; }
    }

    public class Output : Template
    {
        public class OutColumn : Column
        {
            public bool IsFixedWidth { get; set; }
            public int MaxWidth { get; set; }
            public Pad Pad { get; set; }
            public bool IsDateTime { get; set; }
            public string DateTimeFormat { get; set; }
        }

        public string HeaderSeperator { get; set; }
        public string RowHeader { get; set; }
        public string RowTerminator { get; set; }
        public int ColumnPadding { get; set; }
        public string TruncatedMarker { get; set; }
        public List<OutColumn> Columns { get; set; }
    }

}
