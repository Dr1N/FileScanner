using System;

namespace FileScanner.Interfaces
{
    // internal for Moq tests
    internal interface IPrinter
    {
        /// <summary>
        /// Print message for user
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        void Print(string message, ConsoleColor color);
    }
}
