#pragma once
#include <vector>
#include <stdexcept>
#include <cstdlib>

using namespace std;

class ArrayManager
{
private:
    // Внутреннее хранилище чисел
    vector<int> numbers;

public:
    // Создание и заполнение массива случайными числами
    void CreateArray(int size);

    // Проверка, отсортирован ли массив по возрастанию
    bool IsSorted() const;

    // Доступ к вектору чисел (можно менять элементы)
    vector<int>& GetNumbers();
};
