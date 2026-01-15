#pragma once

// Сапожниковым Ю.С.
// Используемые библиотеки
#include "ArrayManager.h"

using namespace std;

template <typename T>
class producer {
public:
    // Конструктор класса, принимающий размер массива
    producer(int n) : N(n) {}

    // Метод создаёт объект ArrayManager и заполняющий массив
    ArrayManager Produce() {
        ArrayManager Array; // Создание локальной пересенной

        Array.CreateArray(N);

        return Array;
    }

private:
    int N;
};
