// Сапожниковым Ю.С.
// Используемые библиотеки
#include "ArrayManager.h"

using namespace std;

    // Создание массива 
void ArrayManager::CreateArray(int size)
    {
        if (size <= 0)
            throw invalid_argument("Размер массива должен быть положительным числом.");

        numbers.resize(size);
        for (int i = 0; i < size; i++)
            numbers[i] = rand() % 9999;
    }

    // Проверка на сортировку
bool ArrayManager::IsSorted() const
    {
        if (numbers.empty())
            throw invalid_argument("Массив не инициализирован.");

        for (int i = 1; i < numbers.size(); i++)
        {
            if (numbers[i - 1] > numbers[i])
            {
                return false;
            }
        }
        return true;
    }
    // Возращает ссылку на числа
vector<int>& ArrayManager::GetNumbers()
    {
        return numbers;
    }

