using System.Collections.Generic;
using System.IO;

namespace FileScenner.Interfaces
{
    interface IFileSystemObjectsProvider
    {
        IEnumerable<FileSystemInfo> EnumerateFileSystemInfos();
    }
}
