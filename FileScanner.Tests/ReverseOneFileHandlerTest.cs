using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;
using System.IO;

namespace FileScanner.Tests
{
    [TestClass]
    public class ReverseOneFileHandlerTest
    {
        [DataRow("file.txt", "file.txt")]
        [DataRow("Directory\\file.txt", "file.txt\\Directory")]
        [DataRow("Directory\\Subdirectory\\file.txt", "file.txt\\Subdirectory\\Directory")]
        [DataRow(" file.txt ", "file.txt")]
        [DataRow(" Directory\\file.txt  ", "file.txt\\Directory")]
        [DataRow(" Directory\\Subdirectory\\file.txt ", "file.txt\\Subdirectory\\Directory")]
        [DataRow("\\file.txt", "file.txt")]
        [DataRow("\\Directory\\file.txt", "file.txt\\Directory")]
        [DataRow("\\Directory\\Subdirectory\\file.txt\\", "file.txt\\Subdirectory\\Directory")]
        [DataTestMethod]
        public void ProcessFile_ValidParameters_Test(string path, string result)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new ReverseOneFileHandler(printerMock.Object);
            var current = handler.ProcessFile(path);

            Assert.IsTrue(current == result);
        }

        [DataRow("")]
        [DataRow(null)]
        [DataRow(" ")]
        [DataRow("\\")]
        [DataTestMethod]
        public void ProcessFile_InvalidParameters_Test(string path)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new ReverseOneFileHandler(printerMock.Object);

            var current = handler.ProcessFile(path);

            Assert.IsNull(current);
        }
    }
}
