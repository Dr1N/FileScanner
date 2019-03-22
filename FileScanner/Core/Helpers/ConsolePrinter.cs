using FileScanner.Interfaces;
using System;

namespace FileScanner.Core
{
    class ConsolePrinter : IPrinter
    {
        public void Print(string message, ConsoleColor color)
        {
            if (message == null)
            {
                return;
            }
            try
            {
                lock (typeof(Console))
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}
