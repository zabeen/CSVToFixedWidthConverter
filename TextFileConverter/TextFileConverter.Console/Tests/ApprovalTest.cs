using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using System;
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
            var converter = new Converter($"{TestContext.CurrentContext.TestDirectory}\\Files\\input.csv");
            converter.ConvertCsvToFixedWidth($"{TestContext.CurrentContext.TestDirectory}\\Files\\output.txt");
            Approvals.Verify(converter.Output);
        }
    }
}
