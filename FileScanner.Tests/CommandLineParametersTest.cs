using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileScanner.Core;
using System.IO;

namespace FileScanner.Tests
{
    [TestClass]
    public class CommandLineParametersTest
    {
        private string _tempPath;

        [TestInitialize]
        public void Setup()
        {
            _tempPath = Path.GetTempPath();
        }

        [DataRow("-s C:\\Windows")]
        [DataRow("-start C:\\Windows")]
        [DataTestMethod]
        public void MakeParameters_DefaultValues_Test(string startPath)
        {
            string[] args = { startPath };

            var result = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(result);
            Assert.AreEqual("all", result.ActionStr);
            Assert.AreEqual("results.txt", result.ResultFile);
        }

        [DataRow(null, null, null)]
        [DataRow("", "", "")]
        [DataRow(" ", " ", " ")]
        [DataRow("-s", "-a", "-r")]
        [DataRow("-s C:\\Windows", "-a", "-r")]
        [DataRow("-s", "-a all", "-r")]
        [DataRow("-s", "-a", "-r results.txt")]
        [DataRow("-s C:\\Windows", "-a all", "-r")]
        [DataRow("-s", "-a all", "-r result.txt")]
        [DataRow("-s C:\\Windows", "-a", "-r result.txt")]
        [DataTestMethod]
        public void MakeParameters_InvalidValues_Test(string startPath, string action, string file)
        {
            string[] args = { startPath, action, file };

            var result = CommandLineParameters.MakeParameters(args);

            Assert.IsNull(result);
        }

        [DataRow("-s C:\\Windows", "-a all", "-r result.txt")]
        [DataRow("-s C:\\Windows\\", "-a cs", "-r result.txt")]
        [DataRow("-s C:\\Windows", "-a reversed1", "-r result.log")]
        [DataRow("-s C:\\Windows", "-a reversed2", "-r result.dat")]
        [DataRow("-start C:\\Windows", "-action all", "-result result.txt")]
        [DataRow("-start C:\\Windows", "-action cs", "-result result.txt")]
        [DataRow("-start C:\\Windows", "-action reversed1", "-result result.log")]
        [DataRow("-start C:\\Windows", "-action reversed2", "-result result.dat")]
        [DataRow("-start C:\\Windows\\", "-action reversed2", "-result result.dat")]
        [DataTestMethod]
        public void MakeParameters_ValidValues_Test(string startPath, string action, string file)
        {
            string[] args = { startPath, action, file };

            var result = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(result);
            StringAssert.EndsWith(startPath, result.StartDirectory);
            StringAssert.EndsWith(action, result.ActionStr);
            StringAssert.EndsWith(file, result.ResultFile);
        }

        [DataRow("all", Action.All)]
        [DataRow("All", Action.All)]
        [DataRow("cs", Action.Cs)]
        [DataRow("Cs", Action.Cs)]
        [DataRow("reversed1", Action.ReversedOne)]
        [DataRow("Reversed1", Action.ReversedOne)]
        [DataRow("reversed2", Action.ReversedTwo)]
        [DataRow("Reversed2", Action.ReversedTwo)]
        [DataTestMethod]
        public void GetAction_ConvertFromString_Test(string action, Action result)
        {
            string[] args = { $"-s {_tempPath}", $"-a {action}" };

            var options = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(options);
            Assert.IsTrue(options.GetAction() == result);
        }

        [DataRow("all")]
        [DataRow(" all ")]
        [DataRow("cs")]
        [DataRow("cs ")]
        [DataRow("reversed1")]
        [DataRow(" reversed1")]
        [DataRow("reversed2")]
        [DataRow("reversed2 ")]
        [DataTestMethod]
        public void IsValid_Action_Success_Test(string action)
        {
            string[] args = { $"-s {_tempPath}", $"-a {action}" };

            var options = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(options);
            Assert.IsTrue(options.IsValid());
        }

        [DataRow("all1")]
        [DataRow("cs1")]
        [DataRow("abc")]
        [DataRow("")]
        [DataRow("  ")]
        [DataTestMethod]
        public void IsValid_Action_Fail_Test(string action)
        {
            string[] args = { $"-s {_tempPath}", $"-a {action}" };

            var options = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(options);
            Assert.IsFalse(options.IsValid());
        }

        [DataRow("console")]
        [DataRow("Console ")]
        [DataRow(" console ")]
        [DataRow(" coNsoLe ")]
        [DataTestMethod]
        public void IsConsole_Success_Test(string console)
        {
            string[] args = { $"-s {_tempPath}", $"-r {console}" };

            var options = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(options);
            Assert.IsTrue(options.IsConsole());
        }

        [DataRow(" ")]
        [DataRow("")]
        [DataRow("abc")]
        [DataRow("res.txt")]
        [DataTestMethod]
        public void IsConsole_Fail_Test(string console)
        {
            string[] args = { $"-s {_tempPath}", $"-a {console}" };

            var options = CommandLineParameters.MakeParameters(args);

            Assert.IsNotNull(options);
            Assert.IsFalse(options.IsConsole());
        }
    }
}