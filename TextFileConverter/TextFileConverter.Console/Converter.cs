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
        private Input _inTemplate;
        private Output _outTemplate;

        public Converter(Input inTemplate, Output outTemplate)
        {
            try
            {
                SetTemplates(inTemplate, outTemplate);
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
                var lines = ReadInputFile(inPath);
                WriteOutputFile(outPath, lines);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when converting input file to output file.", ex);
            }
        }

        private void SetTemplates(Input inTemplate, Output outTemplate)
        {
            if (inTemplate.Columns.Count != outTemplate.Columns.Count)
                throw new ArgumentException("Number of columns in input and output templates must match.");

            _inTemplate = inTemplate;
            _outTemplate = outTemplate;
        }

        private string[] ReadInputFile(string path)
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

            return File.ReadAllLines(path);
        }

        private void WriteOutputFile(string outPath, string[] lines)
        {
            var output = new List<string>();

            if (_outTemplate.IsFirstLineHeader)
            {
                output.Add(CreateNewLine(_outTemplate.Columns.Select(c => c.HeaderText).ToArray(), true));
                output.Add(_outTemplate.HeaderSeperator);
            }

            for (int lineNo = 0; lineNo < lines.Length; lineNo++)
            {
                if (lineNo == 0 && _inTemplate.IsFirstLineHeader)
                    continue;

                output.Add(CreateNewLine(lines[lineNo].Split(_inTemplate.ColumnSeperator), false));
            }

            File.WriteAllLines(outPath, output);
        }

        private string CreateNewLine(string[] cells, bool isHeaderText)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                FormatCellContent(ref cells[i], isHeaderText, _inTemplate.Columns[i], _outTemplate.Columns[i]);
            }

            var sep = _outTemplate.ColumnSeperator.ToString();

            return $"{_outTemplate.RowHeader}{string.Join(sep, cells)}{_outTemplate.RowTerminator}";
        }

        private void FormatCellContent(ref string str, bool isHeaderText, Input.InColumn inCol, Output.OutColumn outCol)
        {
            str = str.Trim();

            if (!isHeaderText && outCol.IsDateTime)
            {
                try
                {
                    var inCulture = CreateCultureInfo(inCol.CultureName);
                    var outCulture = CreateCultureInfo(outCol.CultureName);

                    DateTime strDt = Convert.ToDateTime(str, inCulture);
                    str = strDt.ToString(outCol.DateTimeFormat, outCulture);
                }
                catch (FormatException)
                {
                    throw new FormatException($"Could not convert '{str}' to a valid date when using culture '{inCol.CultureName}'.");
                }              
            }

            if (outCol.IsFixedWidth)
            {
                if (str.Length > outCol.MaxWidth)
                    str = $"{str.Substring(0, outCol.MaxWidth - _outTemplate.TruncatedMarker.Length)}{_outTemplate.TruncatedMarker}";
                else if (str.Length < outCol.MaxWidth)
                    str = (outCol.Pad == Pad.Left) ? str.PadLeft(outCol.MaxWidth) : str.PadRight(outCol.MaxWidth);
            }

            str = str.PadRight(str.Length + _outTemplate.ColumnPadding);
            str = str.PadLeft(str.Length + _outTemplate.ColumnPadding);
        }

        private CultureInfo CreateCultureInfo(string cultureName)
        {
            try
            {
                return (cultureName == null)
                    ? CultureInfo.CurrentCulture
                    : new CultureInfo(cultureName);
            }
            catch (CultureNotFoundException ex)
            {
                throw new CultureNotFoundException($"{ex.InvalidCultureName} is an invalid culture name.");
            }
        }
    }
}
