using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;

namespace FileScanner.Tests
{
    [TestClass]
    public class CsFileHandlerTest
    {
        [DataRow("file.cs", "file.cs /")]
        [DataRow(" file.cs ", "file.cs /")]
        [DataRow("Directory\\file.cs", "Directory\\file.cs /")]
        [DataRow(" Directory\\file.cs ", "Directory\\file.cs /")]
        [DataRow("Directory\\Subdirectory\\file.cs", "Directory\\Subdirectory\\file.cs /")]
        [DataRow(" Directory\\Subdirectory\\file.cs ", "Directory\\Subdirectory\\file.cs /")]
        [DataRow("file.css ", null)]
        [DataRow("Directory\\file.cstxt", null)]
        [DataRow("Directory\\Subdirectory\\file", null)]
        [DataTestMethod]
        public void ProcessFile_ValidParameters_Test(string path, string result)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new CsFileHandler(printerMock.Object);

            var current = handler.ProcessFile(path);

            Assert.IsTrue(current == result);
        }

        [DataRow("")]
        [DataRow(null)]
        [DataRow(" ")]
        [DataRow("file.txt")]
        [DataRow("file.log")]
        [DataRow(" file.exe")]
        [DataTestMethod]
        public void ProcessFile_InvalidParameters_NoWrite_Test(string path)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new CsFileHandler(printerMock.Object);

            var current = handler.ProcessFile(path);

            Assert.IsNull(current);
        }
    }
}