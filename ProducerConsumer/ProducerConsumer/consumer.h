#pragma once

// Сапожниковым Ю.С.
// Используемые библиотеки
#include <algorithm>
#include <stdexcept>
#include "ArrayManager.h"

using namespace std;

template <typename T>
class consumer {
public:
    // Метод, который принимает ссылку на ArrayManager и сортирует его по возрастанию
    void Consume(ArrayManager& Array)
    {
        vector<int>& data = Array.GetNumbers();

        if (data.empty())
            throw invalid_argument("Массив пуст, сортировать нечего.");

        sort(data.begin(), data.end());
    }
};

