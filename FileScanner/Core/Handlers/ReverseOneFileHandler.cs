using FileScanner.Interfaces;
using System.IO;
using System.Linq;

namespace FileScanner.Core
{
    class ReverseOneFileHandler : BaseFileHandler
    {
        public ReverseOneFileHandler(IPrinter printer) : base(printer) { }

        protected override string Process(string filePath)
        {
            string result = null;

            var pathsParts = filePath
                .Trim()
                .Trim(Path.DirectorySeparatorChar)
                .Split(Path.DirectorySeparatorChar)
                .Where(p => !string.IsNullOrEmpty(p))
                .Reverse()
                .ToList();
            if (pathsParts.Count > 0)
            {
                var separator = new string(Path.DirectorySeparatorChar, 1);
                result = string.Join(separator, pathsParts).Trim(Path.DirectorySeparatorChar);
            }

            return result;
        }
    }
}