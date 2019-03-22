using System.Threading;
using System.Threading.Tasks;

namespace FileScanner.Interfaces
{
    interface IDirectoryHandler
    {
        /// <summary>
        /// Handle directory
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProcessDirectoryAsync(CancellationToken cancellationToken);
    }
}