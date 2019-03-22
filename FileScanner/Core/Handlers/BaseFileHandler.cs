using FileScanner.Interfaces;
using System;

namespace FileScanner.Core
{
    abstract class BaseFileHandler : IFileHandler
    {
        protected IPrinter _printer;

        public BaseFileHandler(IPrinter printer)
        {
            _printer = printer;
        }

        public string ProcessFile(string filePath)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    return Process(filePath);
                }
                catch (Exception ex)
                {
                    _printer?.Print($"File processing error: {ex.Message}", ConsoleColor.Red);
                }
            }

            return result;
        }

        protected abstract string Process(string filePath);
    }
}