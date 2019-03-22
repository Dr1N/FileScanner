using FileScanner.Interfaces;

namespace FileScanner.Core
{
    class SimpleFileHandler : BaseFileHandler
    {
        public SimpleFileHandler(IPrinter printer) : base (printer) { }

        protected override string Process(string filePath)
        {
            return filePath.Trim();
        }
    }
}