using System;

namespace Csharp_async
{
    public static class ArrayHelper
    {
        public static bool IsSorted(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] > array[i])
                    return false;
            }
            return true;
        }

        public static int[] MergeArrays(int[] a, int[] b)
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