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
        public string InFileLocation { get; }
        public string Output => _output;

        private string _output;

        public Converter(string inFileLocation)
        {
            if (File.Exists(inFileLocation))
            {
                InFileLocation = inFileLocation;
            }
            else
            {
                throw new FileNotFoundException($"Input file \"{inFileLocation}\" was not found.");
            }
        }

        public void ConvertCsvToFixedWidth(string outFileLocation)
        {
            if (File.Exists(InFileLocation))
            {
                try
                {
                    // read in file contents and split each line by comma delimiter
                    List<string[]> lines = File.ReadLines(InFileLocation).Select(line => line.Split(',')).ToList();

                    // convert values in each string[] to fixed width
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new FileNotFoundException($"Input file \"{InFileLocation}\" was not found.");
            }
        }
    }
}
