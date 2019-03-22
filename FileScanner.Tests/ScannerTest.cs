using System;
using FileScanner.Core;
using FileScanner.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FileScanner.Tests
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_DirectoryHandler_Exception_Test()
        {
            var scanner = new Scanner(null, Mock.Of<IPrinter>());
        }

        [TestMethod]
        public void Ctor_Null_Printer_NoException_Test()
        {
            var scanner = new Scanner(Mock.Of<IDirectoryHandler>(), null);
        }

        [TestMethod]
        public void Check_Run_Disposed_Exception_Test()
        {
            var scanner = new Scanner(Mock.Of<IDirectoryHandler>(), null);
            scanner.Dispose();

            Assert.ThrowsExceptionAsync<ObjectDisposedException>(() => scanner.Run());
        }

        [TestMethod]
        public void Check_Cancel_Disposed_Exception_Test()
        {
            var scanner = new Scanner(Mock.Of<IDirectoryHandler>(), null);
            scanner.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => scanner.Cancel());
        }
    }
}
