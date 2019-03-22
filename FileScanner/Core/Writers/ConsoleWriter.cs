using FileScanner.Interfaces;
using System;

namespace FileScanner.Core
{
    class ConsoleWriter : IResultWriter
    {
        public void Write(string message)
        {
            if (message != null)
            {
                lock (typeof(Console))
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}