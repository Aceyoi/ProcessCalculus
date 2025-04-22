using System;
using System.Threading.Tasks;                      
using System.Windows;
using System.ComponentModel;
using System.Threading;

namespace Csharp_async
{
    public partial class MainWindow : Window
    {
        IntegralCalculator integralCalc = new IntegralCalculator();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double a = 0;                 // Нижний предел интегрирования
            double b = 10;                 // Верхний предел интегрирования
            ulong n = Convert.ToUInt64(Convert.ToDouble(textbox_n.Text)) * 1000_000;  // Количество разбиений

            progressBar.Value = 0;
            textbox_result.Text = " ";

            var progress = new Progress<int>(percent =>
            {
                progressBar.Value = percent;
            });

            double result = integralCalc.CalculateIntegral(a, b, n, progress);
            textbox_result.Text = result.ToString();
        }

        private async void btn_run_sep_thread_Click(object sender, RoutedEventArgs e)
        {
            double a = 0;                 // Нижний предел интегрирования
            double b = 10;                 // Верхний предел интегрирования
            ulong n = Convert.ToUInt64(Convert.ToDouble(textbox_n.Text)) * 1000_000;  // Количество разбиений

            btn_run_sep_thread.IsEnabled = false;
            progressBar.Value = 0;
            textbox_result.Text = " ";

            var progress = new Progress<int>(percent =>
            {
                progressBar.Value = percent;
            });

            double result = await Task.Run(() => integralCalc.CalculateIntegral(a, b, n, progress));

            textbox_result.Text = result.ToString();
            btn_run_sep_thread.IsEnabled = true;
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            integralCalc.Stop = true;
        }
    }
}