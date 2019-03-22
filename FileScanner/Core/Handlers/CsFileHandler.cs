using FileScanner.Interfaces;
using System.IO;

namespace FileScanner.Core
{
    class CsFileHandler : BaseFileHandler
    {
        private const string CsExtension = ".cs";
        private const string Suffix = " /";

        public CsFileHandler() : base() { }
       
        public CsFileHandler(IPrinter printer) : base(printer) { }

        protected override string Process(string filePath)
        {
            string result = null;

            var fileExtension = Path.GetExtension(filePath);
            if (fileExtension.Trim().ToUpper() == CsExtension.ToUpper())
            {
                result = filePath.Trim() + Suffix;
            }

            return result;
        }
    }
}