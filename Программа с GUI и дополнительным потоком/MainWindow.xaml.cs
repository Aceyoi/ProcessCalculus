// Cапожников Юрий ИВТ-22
using System;
using System.Windows;
using System.Threading.Tasks;

namespace Csharp_async
{
    public partial class MainWindow : Window
    {
        private ArrayManager arrayManager = new ArrayManager();
        private SortAlgorithms sortAlgorithms = new SortAlgorithms();
        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Создание массива
        private void btn_fill_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textbox_n.Text, out int size) && size > 0)
            {
                arrayManager.CreateArray(size);
                textbox_result.Text = $"Массив из {size} элементов создан.";
            }
            else
            {
                textbox_result.Text = "Введите корректный размер массива.";
            }
        }

        private void btn_main_Click(object sender, RoutedEventArgs e)
        {
            if (arrayManager.Numbers == null) return;

            sortAlgorithms.StopRequested = false;
            progressBar.Value = 0;
            stopwatch.Restart();

            sortAlgorithms.BubbleSortWithProgress(arrayManager.Numbers, p => progressBar.Value = p);
            stopwatch.Stop();

            textbox_result.Text = $"Сортировка завершена (основной поток) за {stopwatch.ElapsedMilliseconds} мс.";
        }

        private async void btn_task_Click(object sender, RoutedEventArgs e)
        {
            if (arrayManager.Numbers == null) return;

            sortAlgorithms.StopRequested = false;
            progressBar.Value = 0;

            stopwatch.Restart();
            await Task.Run(() => sortAlgorithms.BubbleSortWithProgress(
                arrayManager.Numbers,
                p => Dispatcher.Invoke(() => progressBar.Value = p)));
            stopwatch.Stop();

            textbox_result.Text = $"Сортировка завершена (отдельный поток) за {stopwatch.ElapsedMilliseconds} мс.";
        }

        private async void btn_multi_Click(object sender, RoutedEventArgs e)
        {
            if (arrayManager.Numbers == null) return;

            if (!int.TryParse(textbox_threads.Text, out int threadCount) || threadCount <= 0)
            {
                textbox_result.Text = "Введите корректное количество потоков.";
                return;
            }

            sortAlgorithms.StopRequested = false;
            progressBar.Value = 0;

            stopwatch.Restart();
            await sortAlgorithms.ParallelSortWithThreadPool(
                arrayManager.Numbers,
                threadCount,
                p => Dispatcher.Invoke(() => progressBar.Value = p));
            stopwatch.Stop();

            textbox_result.Text = $"Сортировка завершена (пул из {threadCount} потоков) за {stopwatch.ElapsedMilliseconds} мс.";
        }

        // Проверка массива
        private void btn_check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (arrayManager.IsSorted())
                {
                    textbox_result.Text = "Массив отсортирован корректно.";
                }
                else
                {
                    textbox_result.Text = "Массив не отсортирован.";
                }
            }
            catch (InvalidOperationException ex)
            {
                textbox_result.Text = ex.Message;
            }
        }

        // Остановка
        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            sortAlgorithms.StopRequested = true;
        }
    }
}