using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileScanner.Core;
using FileScanner.Interfaces;

namespace FileScanner.Tests
{
    [TestClass]
    public class StrPathCalculatorTest
    {
        [TestMethod]
        public void Ctor_NullPrinter_Parameter_Test()
        {
            var calc = new StrPathCalculator(null);
        }

        [TestMethod]
        public void Ctor_NotNullPrinter_Parameter_Test()
        {
            var printer = Mock.Of<IPrinter>();

            var calc = new StrPathCalculator(printer);
        }

        [TestMethod]
        public void NullPrinter_NoException_Test()
        {
            var calc = new StrPathCalculator(null);

            calc.GetRelativePath("", "");
        }

        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow("abc", "")]
        [DataRow("", "abc")]
        [DataRow("def", "abc")]
        [DataRow("C:\\Directory\\test.txt", "D:\\Temp")]
        [DataRow("C:\\Directory\\test.txt", "C:\\Directory\\Sub1")]
        [DataRow("D:\\Directory", "C:\\Directory\\test.txt")]
        [DataRow("Directory\\text.txt", "Directory\\Temp")]
        [DataRow("Directory\\Sub1\\file1.txt", "Directory")]
        [DataRow("\\Directory\\Sub1\\file1.txt", "\\Directory")]
        [DataRow(".\\Directory\\Sub1\\file1.txt", ".\\Directory")]
        [DataTestMethod]
        public void GetRelativePath_Invalid_Parameters_Test(string path1, string path2)
        {
            var printer = Mock.Of<IPrinter>();
            var calc = new StrPathCalculator(printer);

            var result = calc.GetRelativePath(path1, path2);

            Assert.IsNull(result);
        }

        [DataRow("C:\\Directory\\file1.txt", "C:\\Directory", "file1.txt")]
        [DataRow("C:\\Directory\\file1.txt", "C:\\Directory\\", "file1.txt")]
        [DataRow("C:\\Directory\\Subdirectory\\file1.txt ", "C:\\Directory", "Subdirectory\\file1.txt")]
        [DataRow("C:\\Directory\\Subdirectory\\file1.txt", "C:\\Directory\\", "Subdirectory\\file1.txt")]
        [DataRow("C:\\Directory\\Sub1\\file1.txt", "C:\\Directory", "Sub1\\file1.txt")]
        [DataRow("C:\\Directory\\Sub1\\file1.txt", "C:\\Directory\\", "Sub1\\file1.txt")]
        [DataRow("C:\\Directory\\Sub1\\Sub2\\file1.txt", "C:\\Directory", "Sub1\\Sub2\\file1.txt")]
        [DataRow(" C:\\Directory\\Sub1\\Sub2\\file1.txt ", " C:\\Directory\\ ", "Sub1\\Sub2\\file1.txt")]
        [DataTestMethod]
        public void GetRelativePath_Valid_Parameters_Test(string path1, string path2, string result)
        {
            var printer = Mock.Of<IPrinter>();
            var calc = new StrPathCalculator(printer);

            var current = calc.GetRelativePath(path1, path2);

            Assert.IsTrue(result == current);
        }
    }
}