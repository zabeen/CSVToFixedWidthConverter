using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileConverter.Library
{

    public class Converter
    {
        public string Output => string.Join(Environment.NewLine,_output);

        private Input _inTemplate;
        private Output _outTemplate;
        private List<string> _output = new List<string>();

        public Converter(Input inTemplate, Output outTemplate)
        {
            _inTemplate = inTemplate;
            _outTemplate = outTemplate;

            try
            {
                CompareTemplates();
            }
            catch (Exception ex)
            {
                throw new Exception("Error when initialising the file converter.", ex);
            }
        }

        public void ConvertInputToOutput(string inPath, string outPath)
        {
            try
            {
                CheckInputFile(inPath);
                var lines = File.ReadAllLines(inPath);

                if (_outTemplate.IsFirstLineHeader)
                {
                    AddNewLineToOutput(_outTemplate.Columns.Select(c => c.HeaderText).ToArray(), true);
                    _output.Add(_outTemplate.HeaderSeperator);
                }

                for (int lineNo = 0; lineNo < lines.Length; lineNo++)
                {
                    if (lineNo == 0 && _inTemplate.IsFirstLineHeader)
                        continue;

                    AddNewLineToOutput(lines[lineNo].Split(_inTemplate.ColumnSeperator), false);
                }

                File.WriteAllLines(outPath, _output);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when converting input file to output file.", ex);
            }
        }

        private void CompareTemplates()
        {
            if (_inTemplate.Columns.Count != _outTemplate.Columns.Count)
                throw new ArgumentException("Number of columns in input and output templates must match.");
        }

        private void CheckInputFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Input file was not found at path: {path}.");

            var firstLine = File.ReadLines(path).First().Split(_inTemplate.ColumnSeperator);

            if (firstLine.Length != _inTemplate.Columns.Count)
                throw new FileFormatException("Input file has an unexpected number of columns.");

            if (_inTemplate.IsFirstLineHeader)
            {
                for (int i = 0; i < firstLine.Length; i++)
                {
                    if (!firstLine[i].Equals(_inTemplate.Columns[i].HeaderText))
                        throw new FileFormatException($"{firstLine[i]} is an unexpected column header; expecting: {_inTemplate.Columns[i].HeaderText}.");
                }
            }
        }

        private void AddNewLineToOutput(string[] cells, bool isHeader)
        {
            for (int colNo = 0; colNo < _inTemplate.Columns.Count; colNo++)
            {
                FormatStringAsOutput(ref cells[colNo], isHeader, _inTemplate.Columns[colNo], _outTemplate.Columns[colNo]);
            }

            _output.Add(CreateNewLine(cells));
        }

        private string CreateNewLine(IEnumerable<string> content)
        {
            var cells = content.ToArray();
            var sep = _outTemplate.ColumnSeperator.ToString();

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = cells[i].PadRight(cells[i].Length + _outTemplate.ColumnPadding);
                cells[i] = cells[i].PadLeft(cells[i].Length + _outTemplate.ColumnPadding);
            }

            return $"{_outTemplate.RowHeader}{string.Join(sep, cells)}{_outTemplate.RowTerminator}";
        }

        private void FormatStringAsOutput(ref string str, bool isHeader, Input.InColumn inCol, Output.OutColumn outCol)
        {
            str = str.Trim();

            if (!isHeader && outCol.IsDateTime)
            {
                DateTime strDt;

                try
                {
                    strDt = Convert.ToDateTime(str, new CultureInfo(_inTemplate.CultureName));
                }
                catch (FormatException)
                {
                    throw new FormatException($"{str} is not a valid date.");
                }

                str = strDt.ToString(outCol.DateTimeFormat, new CultureInfo(_outTemplate.CultureName));
            }

            if (outCol.IsFixedWidth)
            {
                if (str.Length > outCol.MaxWidth)
                    str = $"{str.Substring(0, outCol.MaxWidth - _outTemplate.TruncatedMarker.Length)}{_outTemplate.TruncatedMarker}";
                else if (str.Length < outCol.MaxWidth)
                    str = (outCol.Pad == Pad.Left) ? str.PadLeft(outCol.MaxWidth) : str.PadRight(outCol.MaxWidth);
            }
        }
    }
}
