using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace FileScanner.Tests
{
    [TestClass]
    public class DirectoryHandlerTest
    {
        private string _startDir;
        private string _invalidDir;
        private Mock<IFileHandler> _fileHandlerMock;
        private Mock<IResultWriter> _resultWriterMock;
        private Mock<IPathCalculator> _pathCalculatorMock;
        private Mock<IPrinter> _printerMock;

        [TestInitialize]
        public void Setup()
        {
            _startDir = Path.GetTempPath();
            _invalidDir = "Fake:\\" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            _fileHandlerMock = new Mock<IFileHandler>();
            _resultWriterMock = new Mock<IResultWriter>();
            _pathCalculatorMock = new Mock<IPathCalculator>();
            _printerMock = new Mock<IPrinter>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_FileHandler_Exception()
        {
            var handler = new DirectoryHandler(_startDir, null, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_ResultWriter_Exception()
        {
            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, null, _pathCalculatorMock.Object, _printerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_PathCaclulator_Exception()
        {
            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, _resultWriterMock.Object, null, _printerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_Null_StartPath_Exception()
        {
            var handler = new DirectoryHandler(null, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_Empty_StartPath_Exception()
        {
            var handler = new DirectoryHandler(string.Empty, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_Invalid_StartPath_Exception()
        {
            var handler = new DirectoryHandler(_invalidDir, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
        }

        [TestMethod]
        public void Ctor_Null_Printer_NoException()
        {
            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, null);
        }

        [DataRow("file.exe")]
        [DataRow("Directory\\file.cs")]
        [DataRow("Directory\\Subdirectory\\file.cs")]
        [DataTestMethod]
        public void ProcessFile_Valid_Data_Test(string path)
        {
            string writedResult = null;
            _fileHandlerMock
                .Setup(fh => fh.ProcessFile(It.IsAny<string>())).Returns<string>(a => path);
            _resultWriterMock
               .Setup(w => w.Write(It.IsAny<string>()))
               .Callback<string>(m => writedResult = m);
            _pathCalculatorMock
                .Setup(pc => pc.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((a, b) => path);

            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
            handler.ProcessFile(path);

            _fileHandlerMock.Verify(fw => fw.ProcessFile(It.IsAny<string>()), Times.Once());
            _resultWriterMock.Verify(fw => fw.Write(It.IsAny<string>()), Times.Once());
            _pathCalculatorMock.Verify(fw => fw.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            Assert.AreEqual(writedResult, path);
        }

        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        [DataTestMethod]
        public void ProcessFile_InValid_Data_Test(string path)
        {
            _fileHandlerMock
                .Setup(fh => fh.ProcessFile(It.IsAny<string>())).Returns<string>(a => path);
            _resultWriterMock
               .Setup(w => w.Write(It.IsAny<string>()));
            _pathCalculatorMock
                .Setup(pc => pc.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((a, b) => path);

            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
            handler.ProcessFile(path);

            _fileHandlerMock.Verify(fw => fw.ProcessFile(It.IsAny<string>()), Times.Never());
            _resultWriterMock.Verify(fw => fw.Write(It.IsAny<string>()), Times.Never());
            _pathCalculatorMock.Verify(fw => fw.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ProcessFile_Print_PathCalculator_Error_Test()
        {
            _pathCalculatorMock
                .Setup(pc => pc.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();

            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object);
            handler.ProcessFile(_startDir);

            _printerMock.Verify(fw => fw.Print(It.IsAny<string>(), It.IsAny<ConsoleColor>()), Times.Once());
        }

        [TestMethod]
        public void ProcessFile_Cancel_Test()
        {
            var provider = new FileSystemObjectsProviderFake(true);
            var handler = new DirectoryHandler(_startDir, _fileHandlerMock.Object, _resultWriterMock.Object, _pathCalculatorMock.Object, _printerMock.Object)
            {
                FileProvider = provider,
            };
            var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
            var controlTask = Task.Delay(TimeSpan.FromMilliseconds(400));

            var task = handler.ProcessDirectoryAsync(cancellationSource.Token);
            Task.WaitAny(controlTask, task);

            Assert.IsTrue(task.Status == TaskStatus.RanToCompletion);
        }
    }
}
