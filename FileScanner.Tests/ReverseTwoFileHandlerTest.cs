using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;

namespace FileScanner.Tests
{
    [TestClass]
    public class ReverseTwoFileHandlerTest
    {
        [DataRow("file.txt", "txt.elif")]
        [DataRow("Directory\\file.txt", "txt.elif\\yrotceriD")]
        [DataRow("Directory\\Subdirectory\\file.txt", "txt.elif\\yrotceridbuS\\yrotceriD")]
        [DataRow(" file.txt ", "txt.elif")]
        [DataRow(" Directory\\file.txt ", "txt.elif\\yrotceriD")]
        [DataRow(" Directory\\Subdirectory\\file.txt ", "txt.elif\\yrotceridbuS\\yrotceriD")]
        [DataRow("\\file.txt", "txt.elif")]
        [DataRow("Directory\\file.txt\\", "txt.elif\\yrotceriD")]
        [DataRow("\\Directory\\Subdirectory\\file.txt\\", "txt.elif\\yrotceridbuS\\yrotceriD")]
        [DataTestMethod]
        public void ProcessFile_ValidParameters_Test(string path, string result)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new ReverseTwoFileHandler(printerMock.Object);

            var current = handler.ProcessFile(path);

            Assert.IsTrue(current == result);
        }

        [DataRow("")]
        [DataRow(null)]
        [DataRow(" ")]
        [DataTestMethod]
        public void ProcessFile_InvalidParameters_Test(string path)
        {
            var printerMock = new Mock<IPrinter>();
            var handler = new ReverseTwoFileHandler(printerMock.Object);

            var current = handler.ProcessFile(path);

            Assert.IsNull(current);
        }
    }
}