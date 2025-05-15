using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Csharp_async
{
    public partial class MainWindow : Window
    {
        private int[] numbers;                 // Основной массив чисел
        private Random rand = new Random();
        private bool stopRequested = false;
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();


        public MainWindow()
        {
            InitializeComponent();
        }

        // Создание массива случайных чисел n размера
        private void btn_fill_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textbox_n.Text, out int size) && size > 0)
            {
                numbers = new int[size];
                for (int i = 0; i < size; i++)
                    numbers[i] = rand.Next(0, 10000);

                textbox_result.Text = $"Массив из {size} элементов создан.";
            }
            else
            {
                textbox_result.Text = "Введите корректный размер массива.";
            }
        }

        // Сортировка массива в основном потоке с использованием метода пузырька
        private void btn_main_Click(object sender, RoutedEventArgs e)
        {
            if (numbers == null) return;

            stopRequested = false;  // Сброс флага остановки
            progressBar.Value = 0; // Сброс прогресс-бара
            stopwatch.Restart();  // Запуск таймера

            SortWithProgress(numbers, p => progressBar.Value = p);
            stopwatch.Stop();

            textbox_result.Text = $"Сортировка завершена (основной поток) за {stopwatch.ElapsedMilliseconds} мс.";
        }

        // Сортировка массива в отдельном потоке с использованием метода пузырька
        private async void btn_task_Click(object sender, RoutedEventArgs e)
        {
            if (numbers == null) return;

            stopRequested = false;
            progressBar.Value = 0;
            IProgress<int> progress = new Progress<int>(p => progressBar.Value = p);

            stopwatch.Restart();
            await Task.Run(() => SortWithProgress(numbers, progress.Report));
            stopwatch.Stop();

            textbox_result.Text = $"Сортировка завершена (отдельный поток) за {stopwatch.ElapsedMilliseconds} мс.";
        }

        // Сортировка массива с использованием пула потоков и ограничением до N параллельных задач
        private async void btn_multi_Click(object sender, RoutedEventArgs e)
        {
            if (numbers == null) return;

            if (!int.TryParse(textbox_threads.Text, out int threadCount) || threadCount <= 0)
            {
                textbox_result.Text = "Введите корректное количество потоков.";
                return;
            }

            stopRequested = false;
            progressBar.Value = 0;
            IProgress<int> progress = new Progress<int>(p => progressBar.Value = p);

            stopwatch.Restart();
            await ParallelSortWithThreadPool(numbers, threadCount, progress.Report);
            stopwatch.Stop();

            textbox_result.Text = $"Сортировка завершена (пул из {threadCount} потоков) за {stopwatch.ElapsedMilliseconds} мс.";
        }

        // Проверка, отсортирован ли массив
        private void btn_check_Click(object sender, RoutedEventArgs e)
        {
            if (numbers == null)
            {
                textbox_result.Text = "Массив не задан.";
                return;
            }

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i - 1] > numbers[i])
                {
                    textbox_result.Text = "Массив не отсортирован.";
                    return;
                }
            }

            textbox_result.Text = "Массив отсортирован корректно.";
        }

        // Сортировка пузырьком с выводом прогресса
        private void SortWithProgress(int[] array, Action<int> reportProgress)
        {
            int[] temp = (int[])array.Clone();
            int total = temp.Length;

            for (int i = 0; i < total - 1; i++)
            {
                if (stopRequested) return;

                for (int j = 0; j < total - i - 1; j++)
                {
                    if (temp[j] > temp[j + 1])
                        (temp[j], temp[j + 1]) = (temp[j + 1], temp[j]);
                }

                reportProgress?.Invoke((i + 1) * 100 / total);  // обновляем прогресс
            }

            Array.Copy(temp, array, total); // копируем отсортированный массив обратно
        }

        // Многопоточная сортировка с пулом задач, ограниченным N потоками
        private async Task ParallelSortWithThreadPool(int[] array, int maxThreads, Action<int> reportProgress)
        {
            int length = array.Length;
            int chunkSize = length / maxThreads;// Размер куска массива на поток
            var chunks = new List<int[]>();
            int remaining = length % maxThreads;// Остаток от деления

            // Разбиваем массив на куски
            int index = 0;
            for (int i = 0; i < maxThreads; i++)
            {
                int size = chunkSize + (i < remaining ? 1 : 0);
                int[] chunk = new int[size];
                Array.Copy(array, index, chunk, 0, size);
                chunks.Add(chunk);
                index += size;
            }

            var semaphore = new SemaphoreSlim(maxThreads);
            int completed = 0;

            var tasks = chunks.Select(async chunk => // Создание задач для каждого куска массива
            {
                await semaphore.WaitAsync();// Ожидание свободного потока
                try
                {
                    if (stopRequested) return;

                    Array.Sort(chunk);
                    int current = Interlocked.Increment(ref completed);
                    reportProgress?.Invoke(current * 100 / chunks.Count);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            await Task.WhenAll(tasks);

            if (stopRequested) return;

            // Последовательное слияние всех отсортированных частей
            int[] result = chunks[0];
            for (int i = 1; i < chunks.Count; i++)
            {
                result = MergeArrays(result, chunks[i]);
            }

            Array.Copy(result, array, array.Length); // результат обратно в исходный массив
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

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            stopRequested = true;
        }

    }
}