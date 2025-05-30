// Cапожников Юрий ИВТ-22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Csharp_async
{
    // Класс отвечающий за сортировку
    public class SortAlgorithms
    {
        public bool StopRequested { get; set; } = false;

        // Сортировка пузырьком с выводом прогресса
        public void BubbleSortWithProgress(int[] array, Action<int> reportProgress)
        {
            int[] temp = (int[])array.Clone();
            int total = temp.Length;

            for (int i = 0; i < total - 1; i++)
            {
                if (StopRequested) return;

                for (int j = 0; j < total - i - 1; j++)
                {
                    if (temp[j] > temp[j + 1])
                        (temp[j], temp[j + 1]) = (temp[j + 1], temp[j]);
                }

                reportProgress?.Invoke((i + 1) * 100 / total);
            }

            Array.Copy(temp, array, total);
        }

        // Многопоточная паррарельная сортировка с пулом задач
        // Task - Представляет асинхронную операцию без результата <-- Создание потока
        // async - Включает асинхронность в методе, позволяет использовать
        public async Task ParallelSortWithThreadPool(int[] array, int maxThreads, Action<int> reportProgress)
        {
            int length = array.Length;// Определяем длину входного массива

            // Вычисляем размер части массива для каждого потока

            int chunkSize = length / maxThreads;  // Вычисляем размер массива на каждый поток
            var chunks = new List<int[]>();      // Список для хранения частей исходного массива
            int remaining = length % maxThreads;// Остаток элементов

            // Разбиваем массив на части для параллельной обработки
            int index = 0;
            for (int i = 0; i < maxThreads; i++)//Максимальное количество потоков
            {
                int size = chunkSize + (i < remaining ? 1 : 0);
                int[] chunk = new int[size];// Создаём массив нужного размера
                Array.Copy(array, index, chunk, 0, size);//// Копируем соответствующую часть
                chunks.Add(chunk);// Добавляем в список
                index += size;
            }

            // Семафор (мьютекс) для ограничения количества одновременно работающих потоков
            var semaphore = new SemaphoreSlim(maxThreads);
            int completed = 0;// Счетчик завершенных задач

            // Создаем задачи для каждой части  
            var tasks = chunks.Select(async chunk =>
            {
                await semaphore.WaitAsync();// Ожидаем свободный поток
                try
                {
                    if (StopRequested) return;
                    // Сортировка
                    Array.Sort(chunk);

                    // Увеличиваем счетчик завершенных задач и отчет о прогрессе
                    int current = Interlocked.Increment(ref completed);
                    reportProgress?.Invoke(current * 100 / chunks.Count);
                }
                finally
                {
                    semaphore.Release();// Освобождаем семафор (мьютекс)
                }
            }).ToArray();

            await Task.WhenAll(tasks);// Ожидаем завершения всех задач

            if (StopRequested) return;

            // Соединяем в один массив
            int[] result = chunks[0];
            for (int i = 1; i < chunks.Count; i++)
            {
                result = MergeArrays(result, chunks[i]);
            }

            Array.Copy(result, array, array.Length);
            reportProgress?.Invoke(100);
        }

        // Слияние двух отсортированных массивов в один
        private int[] MergeArrays(int[] a, int[] b)
        {
            int[] result = new int[a.Length + b.Length];
            int i = 0, j = 0, k = 0;
            while (i < a.Length && j < b.Length)
                result[k++] = (a[i] < b[j]) ? a[i++] : b[j++];

            while (i < a.Length) result[k++] = a[i++];
            while (j < b.Length) result[k++] = b[j++];

            return result;
        }
    }
}