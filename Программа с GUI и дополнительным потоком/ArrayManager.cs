// Cапожников Юрий ИВТ-22
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csharp_async
{
    // Класс для проверки массива
    public class ArrayManager
    {
        private Random rand = new Random();
        public int[] Numbers { get; private set; }

        // Создание массива 
        public void CreateArray(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Размер массива должен быть положительным числом.");

            Numbers = new int[size];
            for (int i = 0; i < size; i++)
                Numbers[i] = rand.Next(0, 10000);
        }

        // Проверка на сортировку
        public bool IsSorted()
        {
            if (Numbers == null)
                throw new InvalidOperationException("Массив не инициализирован.");

            for (int i = 1; i < Numbers.Length; i++)
            {
                if (Numbers[i - 1] > Numbers[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}