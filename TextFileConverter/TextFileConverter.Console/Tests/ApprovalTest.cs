using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextFileConverter.Library;

namespace TextFileConverter.Library.Tests
{
    [TestFixture]
    [UseReporter(typeof(NUnitReporter))]
    public class ApprovalTest
    {
        [Test]
        public void VerifyCsvToFixedWidthConversion()
        {
            var inTemplate = new Input()
            {
                IsFirstLineHeader = true,
                ColumnSeperator = ',',
                Columns = new List<Input.InColumn>()
                {
                    new Input.InColumn(){HeaderText = "Publication Date", CultureName = "en-GB"},
                    new Input.InColumn(){HeaderText = "Title"},
                    new Input.InColumn(){HeaderText = "Authors"}
                }
            };

            var outTemplate = new Output()
            {
                IsFirstLineHeader = true,
                HeaderSeperator = "|=============================================================================|",
                RowHeader = "|",
                RowTerminator = "|",
                ColumnSeperator = '|',
                ColumnPadding = 1,
                TruncatedMarker = "...",
                Columns = new List<Output.OutColumn>()
                {
                    new Output.OutColumn(){HeaderText = "Pub Date", IsFixedWidth = true, MaxWidth = 11, Pad = Pad.Right, IsDateTime = true, CultureName = "en-GB", DateTimeFormat = "dd MMM yyyy"},
                    new Output.OutColumn(){HeaderText = "Title", IsFixedWidth = true, MaxWidth = 27, Pad = Pad.Left},
                    new Output.OutColumn(){HeaderText = "Authors", IsFixedWidth = true, MaxWidth = 31, Pad = Pad.Right}
                }
            };
            var converter = new Converter(inTemplate, outTemplate);

            var inPath = $"{TestContext.CurrentContext.TestDirectory}\\Tests\\input.csv";
            var outPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\output.txt";
            converter.ConvertInputToOutput(inPath, outPath);

            Approvals.Verify(File.ReadAllText(outPath));
        }
    }
}
