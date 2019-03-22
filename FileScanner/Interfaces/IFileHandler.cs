namespace FileScanner.Interfaces
{
    // Internal for Moq test
    internal interface IFileHandler
    {
        /// <summary>
        /// Handle file
        /// </summary>
        /// <param name="filePath">File full path</param>
        string ProcessFile(string filePath);
    }
}
