using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextFileConverter.Console;

namespace TextFileConverter.Console.Tests
{
    [TestFixture]
    [UseReporter(typeof(NUnitReporter))]
    public class ApprovalTest
    {
        [Test]
        public void VerifyFixedWidthOutput()
        {
            var inTemplate = new Input()
            {
                IsFirstLineColumnHeader = true,
                ColumnSeperator = ',',             
                Columns = new List<Column>()
                {
                    new Column(){HeaderText = "Publication Date", IsFixedWidth = false},
                    new Column(){HeaderText = "Title", IsFixedWidth = false},
                    new Column(){HeaderText = "Authors", IsFixedWidth = false}
                }              
            };

            var outTemplate = new Output()
            {
                IsFixedWidth = true,
                LineWidth = 79,
                IsFirstLineColumnHeader = true,
                RowHeader = '|',
                RowTerminator = '|',
                ColumnSeperator = '|',
                ColumnPadding = 1,
                Columns = new List<Column>()
                {
                    new Column(){HeaderText = "Pub Date", IsFixedWidth = true, MaxWidth = 11},
                    new Column(){HeaderText = "Title", IsFixedWidth = true, MaxWidth = 27},
                    new Column(){HeaderText = "Authors", IsFixedWidth = true, MaxWidth = 31}
                }
            };

            var converter = new Converter($"{TestContext.CurrentContext.TestDirectory}\\Files\\input.csv", inTemplate, outTemplate);
            converter.ConvertInputToOutput($"{TestContext.CurrentContext.TestDirectory}\\Files\\output.txt");
            Approvals.Verify(converter.Output);
        }
    }
}
