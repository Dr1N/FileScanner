using FileScanner.Interfaces;
using FileScenner.Core;
using FileScenner.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileScanner.Core
{
    class DirectoryHandler : IDirectoryHandler
    {
        private string _startPath;
        private IFileHandler _fileHandler;
        private IResultWriter _resultWriter;
        private IPathCalculator _pathCalculator;
        private IPrinter _printer;

        // For tests
        internal IFileSystemObjectsProvider FileProvider { get; set; }

        public DirectoryHandler(
            string path,
            IFileHandler handler,
            IResultWriter writer,
            IPathCalculator calculator,
            IPrinter printer)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                throw new ArgumentException(nameof(path));
            }

            _fileHandler = handler ?? throw new ArgumentNullException(nameof(handler));
            _resultWriter = writer ?? throw new ArgumentNullException(nameof(writer));
            _pathCalculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _startPath = Path.GetFullPath(path);
            _printer = printer;
        }

        public Task ProcessDirectoryAsync(CancellationToken token)
        {
            return ProcessDirectoryAsync(_startPath, token);
        }

        private Task ProcessDirectoryAsync(string path, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Task.CompletedTask;
            }

            return Task.Run(async () =>
            {
                IFileSystemObjectsProvider provider = null;
                try
                {
                    provider = FileProvider ?? new FileSystemObjectsProvider(path);
                }
                catch (Exception ex)
                {
                    _printer?.Print($"Directory processing error: {ex.Message}", ConsoleColor.Red);
                    return;
                }

                foreach (var fileInfo in provider.EnumerateFileSystemInfos())
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        if (fileInfo is FileInfo fi)
                        {
                            ProcessFile(fi.FullName);
                        }
                        else if (fileInfo is DirectoryInfo)
                        {
                            await ProcessDirectoryAsync(fileInfo.FullName, token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _printer?.Print($"Directory processing canceled", ConsoleColor.Yellow);
                        break;
                    }
                }
               
            }, token);
        }

        // Internal for test
        internal void ProcessFile(string path)
        {
            try
            {
                var relativePath = _pathCalculator.GetRelativePath(path, _startPath);
                if (!string.IsNullOrWhiteSpace(relativePath))
                {
                    var result = _fileHandler.ProcessFile(relativePath);
                    if (!string.IsNullOrEmpty(result))
                    {
                        _resultWriter.Write(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _printer?.Print($"File in directory processing error: {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}