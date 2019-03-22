using FileScanner.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileScanner.Core
{
    class Scanner : IScanner
    {
        private IDirectoryHandler _handler;
        private IPrinter _printer;
        private CancellationTokenSource _cancellationTokenSource;

        public Scanner(IDirectoryHandler directoryHandler, IPrinter printer)
        {
            _handler = directoryHandler ?? throw new ArgumentNullException(nameof(directoryHandler));
            _printer = printer;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Run()
        {
            CheckDisposed();
            try
            {
                await _handler.ProcessDirectoryAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                _printer?.Print($"Processing error: {ex.Message}", ConsoleColor.Red);
            }
            _printer?.Print("Done!", ConsoleColor.Green);
        }

        public void Cancel()
        {
            CheckDisposed();
            try
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _cancellationTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                _printer?.Print($"Cancelling error: {ex.Message}", ConsoleColor.Red);
            }
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource?.Dispose();
                }

                disposedValue = true;
            }
        }
       
        public void Dispose()
        {
            Dispose(true);
        }

        // Internal for tests
        internal void CheckDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion
    }
}