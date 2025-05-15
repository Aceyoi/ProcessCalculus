/// Модуль для вычислений
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_async
{
    public class IntegralCalculator
    {
        public bool Stop { get; set; } = false;

        /// Вычисляет интеграл функции f(x) = x^2 методом прямоугольников
        public double CalculateIntegral(double a, double b, ulong n, IProgress<int> progress)
        {
            double sum = 0;
            double step = (b - a) / n;

            for (ulong i = 0; i < n; i++)
            {
                if (Stop)
                {
                    Stop = false;
                    return 0;
                }

                double x = a + i * step;
                sum += x * x * step;  // f(x) = x²

                // Отчет о прогрессе каждые 1%
                if (i % (n / 100) == 0)
                {
                    progress?.Report((int)(i * 100 / n));
                }
            }//

            progress?.Report(100);
            return sum;
        }
    }
}