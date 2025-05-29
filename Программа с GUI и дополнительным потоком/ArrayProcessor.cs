using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Csharp_async
{
    public class ArrayProcessor
    {
        public event EventHandler<ProgressEventArgs> ProgressChanged;
        public event EventHandler<OperationCompletedEventArgs> OperationCompleted;

        private bool _stopRequested;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void RequestStop() => _stopRequested = true;

        public async Task SortInMainThreadAsync(int[] array)
        {
            ResetState();
            _stopwatch.Start();

            await Task.Run(() =>
            {
                SortWithProgress(array, p => ProgressChanged?.Invoke(this, new ProgressEventArgs(p)));
            });

            CompleteOperation($"Сортировка завершена (основной поток) за {_stopwatch.ElapsedMilliseconds} мс.");
        }

        public async Task SortInBackgroundAsync(int[] array)
        {
            ResetState();
            _stopwatch.Start();

            await Task.Run(() =>
            {
                SortWithProgress(array, p => ProgressChanged?.Invoke(this, new ProgressEventArgs(p)));
            });

            CompleteOperation($"Сортировка завершена (отдельный поток) за {_stopwatch.ElapsedMilliseconds} мс.");
        }

        public async Task SortInParallelAsync(int[] array, int maxThreads)
        {
            ResetState();
            _stopwatch.Start();

            await ParallelSortWithThreadPool(array, maxThreads, p => ProgressChanged?.Invoke(this, new ProgressEventArgs(p)));

            CompleteOperation($"Сортировка завершена (пул из {maxThreads} потоков) за {_stopwatch.ElapsedMilliseconds} мс.");
        }

        private void ResetState()
        {
            _stopRequested = false;
            _stopwatch.Reset();
            ProgressChanged?.Invoke(this, new ProgressEventArgs(0));
        }

        private void CompleteOperation(string message)
        {
            _stopwatch.Stop();
            OperationCompleted?.Invoke(this, new OperationCompletedEventArgs(message));
        }

        private void SortWithProgress(int[] array, Action<int> reportProgress)
        {
            // Реализация сортировки с прогрессом
        }

        private async Task ParallelSortWithThreadPool(int[] array, int maxThreads, Action<int> reportProgress)
        {
            // Реализация параллельной сортировки
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public int Progress { get; }
        public ProgressEventArgs(int progress) => Progress = progress;
    }

    public class OperationCompletedEventArgs : EventArgs
    {
        public string Message { get; }
        public OperationCompletedEventArgs(string message) => Message = message;
    }
}