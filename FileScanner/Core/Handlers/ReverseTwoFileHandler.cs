using FileScanner.Interfaces;
using System.IO;
using System.Linq;

namespace FileScanner.Core
{
    class ReverseTwoFileHandler : BaseFileHandler
    {
        public ReverseTwoFileHandler(IPrinter printer) : base(printer) { }

        protected override string Process(string filePath)
        {
            string result = null;
            var reverse = filePath
                .Trim()
                .Trim(Path.DirectorySeparatorChar)
                .Reverse();
            result = string.Join("", reverse);

            return result;
        }
    }
}