using FileScanner.Interfaces;
using System;
using System.IO;

namespace FileScanner.Core
{
    class StrPathCalculator : IPathCalculator
    {
        private IPrinter _printer;

        public StrPathCalculator() { }

        public StrPathCalculator(IPrinter printer)
        {
            _printer = printer;
        }

        public string GetRelativePath(string targetPath, string basePath)
        {
            string result = null;
            try
            {
                var isNotEmpty = !string.IsNullOrWhiteSpace(targetPath) && !string.IsNullOrWhiteSpace(basePath);
                if (isNotEmpty)
                {
                    targetPath = targetPath.Trim();
                    basePath = basePath.Trim();
                    var isAbsolute = IsAbsolutePath(targetPath) && IsAbsolutePath(basePath);
                    if (isAbsolute)
                    {
                        if (targetPath.StartsWith(basePath))
                        {
                            var basePathLen = basePath.Length;
                            result = targetPath.Substring(basePathLen).Trim(Path.DirectorySeparatorChar);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _printer?.Print($"Relative path calculating error: {ex.Message}", ConsoleColor.Red);
            }

            return result;
        }

        /// https://stackoverflow.com/questions/5565029/check-if-full-path-given
        private bool IsAbsolutePath(string path)
        {
            var result = false;
            try
            {
                result = path == Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                _printer?.Print($"Check absolute path error: {ex.Message}", ConsoleColor.Red);
                result = false;
            }

            return result;
        }
    }
}
