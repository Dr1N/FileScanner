using FileScanner.Interfaces;
using System;
using System.IO;

namespace FileScanner.Core
{
    class FileWriter : IResultWriter
    {
        private string _filePath;
        private object _fileLock;
        private IPrinter _printer;

        public FileWriter(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }
            _fileLock = new object();
            _filePath = filePath.Trim();
        }

        public FileWriter(string filePath, IPrinter printer) : this (filePath)
        {
            _printer = printer;
        }

        public void Write(string message)
        {
            if (message == null)
            {
                return;
            }
            try
            {
                lock (_fileLock)
                {
                    if (!File.Exists(_filePath))
                    {
                        using (File.Create(_filePath)) { } // Dispose file stream and create file
                    }
                    File.AppendAllText(_filePath, $"{message}{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
               _printer?.Print($"Write to file error: {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}