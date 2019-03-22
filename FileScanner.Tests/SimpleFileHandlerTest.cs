using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;

namespace FileScanner.Tests
{
    [TestClass]
    public class SimpleFileHandlerTest
    {
        [DataRow("file.txt")]
        [DataRow("  file.txt  ")]
        [DataRow("Directory\\file.txt")]
        [DataRow(" Directory\\file.txt ")]
        [DataRow("Directory\\Subdirectory\\file.txt")]
        [DataRow(" Directory\\Subdirectory\\file.txt ")]
        [DataTestMethod]
        public void ProcessFile_ValidParameters_Test(string path)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new SimpleFileHandler(printerMock.Object);

            var result = handler.ProcessFile(path);

            Assert.IsTrue(path.Trim() == result);
        }

        [DataRow("")]
        [DataRow(null)]
        [DataRow(" ")]
        [DataTestMethod]
        public void ProcessFile_InvalidParameters_Test(string path)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new SimpleFileHandler(printerMock.Object);

            var result = handler.ProcessFile(path);

            Assert.IsNull(result);
        }
    }
}
