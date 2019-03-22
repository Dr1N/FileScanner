using System.Threading;
using System.Threading.Tasks;

namespace FileScanner.Interfaces
{
    interface IDirectoryHandler
    {
        /// <summary>
        /// Processing files and sub directories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessDirectoryAsync(CancellationToken cancellationToken);
    }
}