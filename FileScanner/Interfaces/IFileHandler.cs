namespace FileScanner.Interfaces
{
    // Internal for Moq test
    internal interface IFileHandler
    {
        /// <summary>
        /// Processing file path
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <returns>Processed file path</returns>
        string ProcessFile(string filePath);
    }
}
