using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;

namespace FileScanner.Tests
{
    [TestClass]
    public class FileWriterTest
    {
        private string _fakePath;
        private string _validPath;
        private string _testMessage;

        [TestInitialize]
        public void Setup()
        {
            _fakePath = "FAKE:\\test.log";
            _validPath = Path.Combine(Path.GetTempPath(), "results.txt");
            _testMessage = "Hello, test";
            DeleteTestFile();
        }
    
        [TestCleanup]
        public void Clean()
        {
            DeleteTestFile();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_Null_Path_Exception_Test()
        {
            var fileWriter = new FileWriter(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_InValid_Path_Exception_Test()
        {
            var fileWriter = new FileWriter(" ");
        }

        [TestMethod]
        public void Write_Invalid_Path_NoException_Test()
        {
            var fileWriter = new FileWriter(_fakePath);

            fileWriter.Write(_testMessage);
        }

        [TestMethod]
        public void Write_Invalid_Path_Printer_Message_Test()
        {
            var mock = new Mock<IPrinter>();
            var printerMock = mock.Object;
            var fileWriter = new FileWriter(_fakePath, printerMock);

            fileWriter.Write(_testMessage);

            mock.Verify(pr => pr.Print(It.IsAny<string>(), It.IsAny<ConsoleColor>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Write_To_File_Success_Test()
        {
            var fileWriter = new FileWriter(_validPath);

            fileWriter.Write(_testMessage);

            string result = null;
            try
            {
                result = File.ReadAllText(_validPath);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.IsNotNull(result);
            StringAssert.Contains(result, _testMessage);
        }

        private void DeleteTestFile()
        {
            try
            {
                if (File.Exists(_validPath))
                {
                    File.Delete(_validPath);
                }
            }
            catch { }
        }
    }
}
