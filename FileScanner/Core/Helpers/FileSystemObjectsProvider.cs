using FileScenner.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileScenner.Core
{
    class FileSystemObjectsProvider : IFileSystemObjectsProvider
    {
        private string _path;

        public FileSystemObjectsProvider(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                throw new ArgumentException(nameof(path));
            }
            _path = path;
        }

        public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
        {
            try
            {
                return new DirectoryInfo(_path).EnumerateFileSystemInfos();
            }
            catch
            {
                return new List<FileSystemInfo>();
            }
        }
    }
}
