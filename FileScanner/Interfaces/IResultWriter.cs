namespace FileScanner.Interfaces
{
    // Internal for Moq test
    internal interface IResultWriter
    {
        /// <summary>
        /// Write message to results
        /// </summary>
        /// <param name="message"></param>
        void Write(string message);
    }
}
