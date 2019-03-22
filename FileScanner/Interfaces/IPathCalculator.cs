namespace FileScanner.Interfaces
{
    // Internal for moq test
    internal interface IPathCalculator
    {
        /// <summary>
        /// Calculate relative path. 
        /// Target path must include base path.
        /// </summary>
        /// <param name="targetPath">Target absolute path</param>
        /// <param name="basePath">Base absolute path</param>
        /// <returns>Relative path or null if error</returns>
        string GetRelativePath(string targetPath, string basePath);
    }
}