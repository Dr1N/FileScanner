using FileScenner.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace FileScanner.Tests
{
    class FileSystemObjectsProviderFake : IFileSystemObjectsProvider
    {
        private readonly bool _infinity;
        private readonly string _fileName;

        public string FileName => _fileName;

        public FileSystemObjectsProviderFake()
        {
            _fileName = Path.GetRandomFileName();
        }

        public FileSystemObjectsProviderFake(bool infinity) : this()
        {
            _infinity = infinity;
        }

        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
        {
            if (_infinity)
            {
                while (true)
                {
                    yield return new FileInfo(_fileName);
                }
            }
            else
            {
                yield return new FileInfo(_fileName);
            }
        }
    }
}
