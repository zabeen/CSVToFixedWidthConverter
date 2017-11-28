using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileConverter.Console
{

    public class Converter
    {
        public string Output => _output;

        struct Line
        {
            public string PublicationDate{ get; set; }
            public string Title { get; set; }
            public string Authors { get; set; }
        }

        private string _inFileLocation;
        private Input _inFileTemplate;
        private Output _outFileTemplate;
        private string _output;

        public Converter(string inFileLocation, Input inFileTemplate, Output outFileTemplate)
        {
            _inFileLocation = inFileLocation;
            _inFileTemplate = inFileTemplate;
            _outFileTemplate = outFileTemplate;
        }

        public void ConvertInputToOutput(string outFileLocation)
        {
            try
            {
                CheckInputFile();

                // read in file contents and split each line by comma delimiter
                List<string[]> lines = File.ReadLines(_inFileLocation).Select(line => line.Split(_inFileTemplate.ColumnSeperator)).ToList();

                // convert values in each string[] to fixed width
                foreach (string[] line in lines)
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckInputFile()
        {
            if (!File.Exists(_inFileLocation))
                throw new FileNotFoundException($"Input file \"{_inFileLocation}\" was not found.");

            string[] firstLine = File.ReadLines(_inFileLocation).First().Split(_inFileTemplate.ColumnSeperator);

            if (firstLine.Length != _inFileTemplate.ColumnCount)
                throw new FileFormatException($"Input file \"{_inFileLocation}\" is not of the expected format.");
        }

        private void ConvertStringToFixedWidth(ref string str, Column col)
        {
            str = str.Trim();

            if (str.Length > col.MaxWidth)
                str = $"{str.Substring(0, col.MaxWidth - _outFileTemplate.TruncatedMarker.Length)}{_outFileTemplate.TruncatedMarker}";
            else if (str.Length < col.MaxWidth)
                str = (col.Pad == Pad.Left) ? str.PadLeft(col.MaxWidth) : str.PadRight(col.MaxWidth);
        }


    }
}
