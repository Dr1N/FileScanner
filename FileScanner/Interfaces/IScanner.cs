using System;
using System.Threading.Tasks;

namespace FileScanner.Interfaces
{
    interface IScanner : IDisposable
    {
        /// <summary>
        /// Start work
        /// </summary>
        Task Run();

        /// <summary>
        /// Cancel work
        /// </summary>
        void Cancel();
    }
}